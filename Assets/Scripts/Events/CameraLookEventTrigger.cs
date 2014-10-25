using UnityEngine;
using System.Collections;

public class CameraLookEventTrigger : MonoBehaviour {
	[System.NonSerialized]
	public bool m_active = false;

	public float m_lerpTime;
	public Transform m_cameraLookPosition;
	public ExitTrigger m_exitTrigger;

	private Quaternion startRot;
	private Transform m_cameraTransform;
	private bool m_facingObject;

	// Use this for initialization
	void Start () {
		if( m_exitTrigger == null )
			Debug.LogError( gameObject.name + " is missing its ExitTrigger." );
		if( m_cameraLookPosition == null )
			Debug.LogError( gameObject.name + " is missing its LookTarget." );
	}
	
	// Update is called once per frame
	void Update () {
		if( m_active ) {
			if( m_facingObject ) {
				if( m_exitTrigger.m_hasEntered ) {
					StartCoroutine( "UnfaceObject" );
					m_active = false;
					return;
				}
				m_cameraTransform.LookAt( new Vector3( m_cameraLookPosition.position.x, m_cameraTransform.position.y, m_cameraLookPosition.position.z ) );
			}
		}
	}

	void OnTriggerEnter( Collider col ) {
		if( col.tag == "Player" ) {
			m_cameraTransform = GameObject.Find( "CameraManager" ).transform;
			m_active = true;
			collider.enabled = false;
			StartCoroutine("FaceObject");
		}
	}

	IEnumerator FaceObject() {
		startRot = m_cameraTransform.rotation;
		Quaternion endRot;
		float timer = 0f;

		while( timer <= 1f ) {
			endRot = Quaternion.LookRotation(  new Vector3( m_cameraLookPosition.position.x, m_cameraTransform.position.y, m_cameraLookPosition.position.z )
			                                 - m_cameraTransform.position );

			m_cameraTransform.rotation = Quaternion.Lerp( startRot, endRot, timer );

			timer += Time.deltaTime / m_lerpTime;
			yield return null;
		}

		m_facingObject = true;
	}

	IEnumerator UnfaceObject() {
		Quaternion rot = m_cameraTransform.localRotation;
		Quaternion endRot = Quaternion.Euler(Vector3.zero);
		float timer = 0f;
		
		while( timer <= 1f ) {
			m_cameraTransform.localRotation = Quaternion.Lerp( rot, endRot, timer );
			
			timer += Time.deltaTime / m_lerpTime;
			yield return null;
		}
		m_cameraTransform.localRotation = endRot;

		gameObject.SetActive( false );
	}
}
