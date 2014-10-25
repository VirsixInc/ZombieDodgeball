using UnityEngine;
using System.Collections;

public class AnimEventTrigger : BaseEventTrigger {

	public AnimEventParams[] m_events;

	private EventTimer m_eventTimer;

	// Use this for initialization
	void Start () {
		m_eventTimer = GetComponent<EventTimer>();
	}
	
	// Update is called once per frame
	void Update () {
		if( m_eventTimer.m_active ) {
			foreach( AnimEventParams t_event in m_events ) {
				if ( !t_event.m_activated ) {
					if( m_eventTimer.m_timer >= t_event.m_time ) {
						t_event.m_character.animation.Blend( t_event.m_animationName );
						t_event.m_activated = true;
					}
				}
			}
		}
	}
}
