using UnityEngine;
using System.Collections;

public class FlyingHordeEnemy : BaseHordeEnemy {

	public float m_speed = 2f;
	public float m_floatSpeed = 1f;
	public float m_lerpTime = 1f;

	private Rigidbody m_rigidbody;
	private float m_lerpTimer = 0f;
	private Quaternion m_startRot;
	private bool m_facingNextPoint = true;

	void Awake () {
		m_rigidbody = GetComponent<Rigidbody>();
		m_thisTransform = GetComponent<Transform>();
		m_animator = GetComponent<Animator>();
	}
	
	void Update() {
		if( !m_dead ) {
			if( m_isMoving ) {
				if( Vector3.Distance( m_thisTransform.position, m_nextWaypoint.transform.position ) < 2f && m_nextWaypoint.m_nextWaypoints.Length > 0 ) {
					m_nextWaypoint = m_nextWaypoint.m_nextFlyingWaypoints[Random.Range( 0, m_nextWaypoint.m_nextFlyingWaypoints.Length )];
					m_startRot = m_thisTransform.rotation;
					m_facingNextPoint = false;
				}

				if( !m_facingNextPoint )
					RotateTowards( m_nextWaypoint.transform.position );

				m_rigidbody.MovePosition( m_thisTransform.position + ( m_nextWaypoint.transform.position -  m_thisTransform.position ).normalized * m_speed * Time.deltaTime );
			}
		} else {
			m_rigidbody.MovePosition( m_thisTransform.position + Vector3.up * m_floatSpeed * Time.deltaTime );

			if( m_deathTimer >= m_deathWaitTime ) {
				Reset();
				//TODO: Reduce hp
			}
			
			m_deathTimer += Time.deltaTime;
		}
	}
	
	public override void StartMoving( HordeWaypoint startWp ) {
		m_nextWaypoint = startWp.m_nextFlyingWaypoints[Random.Range( 0, startWp.m_nextFlyingWaypoints.Length )];
		m_isMoving = true;
		m_thisTransform.LookAt( new Vector3( m_nextWaypoint.transform.position.x, m_thisTransform.position.y, m_nextWaypoint.transform.position.z ) );			
	}
	
	protected override void Reset() {
		m_hasBeenHit = false;
		m_facingNextPoint = true;
		m_deathTimer = 0f;
		m_dead = false;
		m_isMoving = false;
		gameObject.SetActive( false );
	}
	
	protected override void Hit( GameObject hittingObj ) {
		if( !m_hasBeenHit ) {
			int pointsToAdd = 1;

			if( hittingObj.name.Contains( "Red" ) )
				PlayerManager.AddPoints(PlayerColor.Red, pointsToAdd);

			else if ( hittingObj.name.Contains( "Yellow" ) )
				PlayerManager.AddPoints(PlayerColor.Yellow, pointsToAdd);

			else if ( hittingObj.name.Contains( "Green" ) )
				PlayerManager.AddPoints(PlayerColor.Green, pointsToAdd);

			else if ( hittingObj.name.Contains( "Blue" ) )
				PlayerManager.AddPoints(PlayerColor.Blue, pointsToAdd);
			else
				Debug.LogError( hittingObj.name + " doesn't have a name containing a supported Color" );

			m_hasBeenHit = true;
			m_isMoving = false;
			m_dead = true;
			GetComponent<Animator>().SetBool( "Dead", true );
			}
	}

	protected void RotateTowards( Vector3 rotatePos ) {
		if( m_lerpTimer >= 1f ) {
			m_lerpTimer = 0f;
			m_facingNextPoint = true;
			return;
		}

		Quaternion endRot;

		endRot = Quaternion.LookRotation(  new Vector3( rotatePos.x, m_thisTransform.position.y, rotatePos.z )
		                                 - m_thisTransform.position );
		
		m_thisTransform.rotation = Quaternion.Lerp( m_startRot, endRot, m_lerpTimer );		
		m_lerpTimer += Time.deltaTime / m_lerpTime;
	}
}
