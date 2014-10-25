using UnityEngine;
using System.Collections;

[System.Serializable]
public class AnimEventParams {

	[System.NonSerialized]
	public bool m_activated = false;

	public float m_time;
	public GameObject m_character;
	public string m_animationName;
}
