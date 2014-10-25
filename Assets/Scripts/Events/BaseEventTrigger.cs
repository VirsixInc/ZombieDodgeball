using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (EventTimer))]
public class BaseEventTrigger : MonoBehaviour {

	public bool m_stopsPlayerMovement = false;
	public bool lookAt = false;

}
