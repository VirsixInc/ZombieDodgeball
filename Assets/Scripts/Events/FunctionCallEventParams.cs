using UnityEngine;
using System.Collections;

[System.Serializable]
public class FunctionCallEventParams {

	[System.NonSerialized]
	public bool m_activated = false;
	
	public float m_time;
	public GameObject m_object;
	public string m_function;
}
