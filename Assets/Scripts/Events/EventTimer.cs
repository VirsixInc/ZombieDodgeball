using UnityEngine;
using System.Collections;

public class EventTimer : MonoBehaviour {

	public bool m_active = false;
	public bool m_finished = false;

	public float m_timer = 0;			// timer the events will reference to
	public float m_duration;			// the amount of time the event will last in total

	void OnTriggerEnter( Collider col ) {
		if( col.tag == "Player" ) {
			m_active = true;
			collider.enabled = false;
		}
	}

	void Update() {
		if( m_active ) {
			if ( m_timer >= m_duration ) {
				m_finished = true;
				m_active = false;
			}

			m_timer += Time.deltaTime;
		}
	}
}
