//#define LevelDebug

using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {
		
	private static CameraMove _instance;

	private Transform m_thisTransform;
	//[System.NonSerialized]
	public bool m_isMoving;
	
	[System.NonSerialized]
	public NavMeshAgent navMeshAgent;
	[System.NonSerialized]
	public NetworkInterpolatedTransform m_NIT;

	private Waypoint m_levelStartWp;
	private Waypoint m_levelEndWp;
	private Waypoint m_nextWaypoint;
	
	public float timeSinceEvent = 0f;

	#region Singleton Initialization
	public static CameraMove instance {
		get { 
			if(_instance == null)
				_instance = GameObject.FindObjectOfType<CameraMove>();
			
			return _instance;
		}
	}
	
	void Awake() {
		if(_instance == null) {
			//If I am the fist instance, make me the first Singleton
			_instance = this;
			DontDestroyOnLoad(gameObject);
		} else {
			//If a Singleton already exists and you find another reference in scene, destroy it
			if(_instance != this)
				DestroyImmediate(gameObject);
				//Destroy(gameObject);
		}
	}

	#endregion

	// Use this for initialization
	void Start () {
		m_thisTransform = GetComponent<Transform>();
		navMeshAgent = GetComponent<NavMeshAgent>();
		m_NIT = GetComponent<NetworkInterpolatedTransform>();
#if !LevelDebug
	}
		
	void OnLevelWasLoaded( int level ) {
#endif
		if( Network.isServer ) {
			MoveToLevelStart();

			if( level > 0 ) {
				GameManager.instance.ChangeMode( (int)GameManager.GameMode.Title );

				// Check if we have set the beginning or end of the the level. If not, cry.
				if( m_levelStartWp == null ) {
					m_levelStartWp = GameObject.FindGameObjectWithTag( "LevelStart" ).GetComponent<Waypoint>();

					if( m_levelStartWp == null )
							Debug.LogError( "CameraMove manager: " + name + " is missing its LevelStart waypoint." );
				}
				if( m_levelEndWp == null ) {
					m_levelEndWp = GameObject.FindGameObjectWithTag( "LevelEnd" ).GetComponent<Waypoint>();
					
					if( m_levelEndWp == null )
						Debug.LogError( "CameraMove manager: " + name + " is missing its LevelEnd waypoint." );
				}
				
				PathCompleteCheck();

				m_nextWaypoint = m_levelStartWp.m_next;
			}
#if !LevelDebug		
		} else {
			if( Network.peerType == NetworkPeerType.Disconnected )
				MoveToLevelStart();
		} 
#endif
	}
	
	// Update is called once per frame
	void Update () {
		if( m_isMoving ) {
			if( Vector3.Distance( m_thisTransform.position, m_nextWaypoint.transform.position ) < 2f 
			   && m_nextWaypoint.isTimedEvent && timeSinceEvent < m_nextWaypoint.eventTime){
				StopMovement();
				timeSinceEvent += Time.deltaTime;
			} else {
				if( GameManager.instance.CurrentMode == (int)GameManager.GameMode.Play ) {
					if( Vector3.Distance( m_thisTransform.position, m_nextWaypoint.transform.position ) < 2f ) {
						if( m_nextWaypoint == m_levelEndWp ) {
							StopMovement();
						} else {
							m_nextWaypoint = m_nextWaypoint.m_next;
							MoveCamAlongSpline();
						}
					}
				}
			}
		}
	}

	void StopMovement() {
		navMeshAgent.Stop();
	}
	
	void PathCompleteCheck() {
		Waypoint temp = m_levelStartWp;
		
		while( temp != null ) {
			// If we run into a point that doesn't have a next and it is LevelEnd, we're good. Otherwise, cry harder.
			if( temp.m_next == null ) {
				if( temp == m_levelEndWp ) {
					Debug.Log( "Path complete." );
					return;
				} else {
					Debug.LogError( "Path is not complete. " + temp.name + " doesn't have a next WayPoint" );
					return;
				}
			}
			
			// Make sure we aren't in an infinite loop
			if( temp.m_next.m_checked ) {
				Debug.LogError( "Infinite loop detected. " + temp.name + " loops back to " + temp.m_next );
				return;
			}
			
			// Activate the flag that says that this object has been checked for path completion
			temp.m_checked = true;
			
			temp = temp.m_next;
		}
	}

	public void MoveToLevelStart() {
		Transform levelStart = GameObject.FindGameObjectWithTag( "LevelStart" ).transform;
		Transform lookDir =  levelStart.GetChild(0).transform;
		m_thisTransform.position = levelStart.position;
		m_thisTransform.LookAt( new Vector3( lookDir.position.x, m_thisTransform.position.y, lookDir.position.z ) );
		ManagerScript.instance.transform.localRotation = Quaternion.identity;
	}
	
	public static void StartMovement() {
		if(Network.isServer)
			_instance.navMeshAgent.SetDestination( _instance.m_nextWaypoint.transform.position );
	}
	
	[RPC]
	public void MoveCamAlongSpline () {
		navMeshAgent.SetDestination( m_nextWaypoint.transform.position );
	}

	public void Reset() {
		m_isMoving = false;
		m_levelStartWp = null;
		m_levelEndWp = null;
		m_nextWaypoint = null;
	}
}
