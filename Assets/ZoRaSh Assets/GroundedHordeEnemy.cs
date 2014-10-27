using UnityEngine;
using System.Collections;

[RequireComponent (typeof ( NavMeshAgent ))]
public class GroundedHordeEnemy : BaseHordeEnemy {

	NavMeshAgent m_navmeshAgent;

	void Awake () {
		m_navmeshAgent = GetComponent<NavMeshAgent>();
		m_thisTransform = GetComponent<Transform>();
		m_animator = GetComponent<Animator>();
	}

	void Update() {
		if( !m_dead ) {
			if( m_isMoving ) {
				if( Vector3.Distance( m_thisTransform.position, m_nextWaypoint.transform.position ) < 2f && m_nextWaypoint.m_nextWaypoints.Length > 0 ) {
					m_nextWaypoint = m_nextWaypoint.m_nextWaypoints[Random.Range( 0, m_nextWaypoint.m_nextWaypoints.Length )];
					m_navmeshAgent.SetDestination( m_nextWaypoint.transform.position );
				}
			}
		} else {
			if( m_deathTimer >= m_deathWaitTime ) {
				Reset();
				//TODO: Reduce hp
			}

			m_deathTimer += Time.deltaTime;
		}
	}
	
	public override void StartMoving( HordeWaypoint startWp ) {
		m_navmeshAgent.enabled = true;
		m_animator.enabled = true;
		SetKinematic( true );
		m_nextWaypoint = startWp.m_nextWaypoints[Random.Range( 0, startWp.m_nextWaypoints.Length )];
		m_navmeshAgent.SetDestination( m_nextWaypoint.transform.position );
		m_isMoving = true;

	}

	protected override void Reset() {
		m_hasBeenHit = false;
		m_deathTimer = 0f;
		m_dead = false;
		m_isMoving = false;
		SetKinematic( true );
		m_animator.enabled = true;
		m_navmeshAgent.enabled = false;
		gameObject.SetActive( false );
	}

	protected override void Hit( GameObject hittingObj ) {
		if( !m_hasBeenHit ) {
			Ball hittingBall = hittingObj.GetComponent<Ball>();


			if( hittingBall.color == PlayerColor.Red ) {
				PlayerManager.AddPoints(PlayerColor.Red, m_pointWorth);
				FloatingTextManager.instance.CreateFloatingText( m_thisTransform.position, m_pointWorth, Color.red );
			}
			else if( hittingBall.color == PlayerColor.Green) {
				PlayerManager.AddPoints(PlayerColor.Green, m_pointWorth);
				FloatingTextManager.instance.CreateFloatingText( m_thisTransform.position, m_pointWorth, Color.green );
			}
			else if( hittingBall.color == PlayerColor.Yellow) {
				PlayerManager.AddPoints(PlayerColor.Yellow, m_pointWorth);
				FloatingTextManager.instance.CreateFloatingText( m_thisTransform.position, m_pointWorth, Color.yellow );
			}
			else if( hittingBall.color == PlayerColor.Blue) {
				PlayerManager.AddPoints(PlayerColor.Blue, m_pointWorth);
				FloatingTextManager.instance.CreateFloatingText( m_thisTransform.position, m_pointWorth, Color.blue );
			}


			SetKinematic( false );
			m_hasBeenHit = true;
			m_animator.enabled = false;
			m_navmeshAgent.Stop();
			m_dead = true;
			GetComponent<Animator>().SetBool( "Dead", true );
		}
	}

	private void SetKinematic( bool value ) {
		Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

		foreach( Rigidbody rB in rigidbodies ) {
			rB.isKinematic = value;
		}
	}
}
