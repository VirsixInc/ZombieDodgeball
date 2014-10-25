using UnityEngine;
using System.Collections;

public class HordeWaypoint : MonoBehaviour {

	public GameObject m_waypointHolder;
	public HordeWaypoint[] m_nextWaypoints;
	public HordeWaypoint[] m_nextFlyingWaypoints;

	void Awake() {
		if( m_waypointHolder != null )
			m_nextWaypoints =  m_waypointHolder.GetComponentsInChildren<HordeWaypoint>();
	}

	void OnDrawGizmosSelected() {
		if( m_nextWaypoints != null ) {
			Gizmos.color = Color.cyan;
			if( m_nextWaypoints.Length > 0 ) {
				foreach( HordeWaypoint wp in m_nextWaypoints ) {
					Gizmos.DrawLine( transform.position, wp.transform.position );
				}
			}
			if( m_nextFlyingWaypoints.Length > 0 ) {
				foreach( HordeWaypoint wp in m_nextFlyingWaypoints ) {
					Gizmos.DrawLine( transform.position, wp.transform.position );
				}
			}
		}
	}
}
