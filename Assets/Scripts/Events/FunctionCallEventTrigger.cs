using UnityEngine;
using System.Collections;

[RequireComponent (typeof (EventTimer))]
public class FunctionCallEventTrigger : MonoBehaviour {

	public FunctionCallEventParams[] m_events;

	private EventTimer m_eventTimer;
	
	// Use this for initialization
	void Start () {
		m_eventTimer = GetComponent<EventTimer>();
	}
	
	// Update is called once per frame
	void Update () {
		if( m_eventTimer.m_active ) {
			foreach( FunctionCallEventParams t_event in m_events ) {
				// if event hasn't been activated
				if ( !t_event.m_activated ) {
					// and it is now time to trigger event
					if( m_eventTimer.m_timer >= t_event.m_time ) {
						t_event.m_object.SendMessage( t_event.m_function, SendMessageOptions.RequireReceiver );
						t_event.m_activated = true;
					}
				}
			}
		}
	}
}

