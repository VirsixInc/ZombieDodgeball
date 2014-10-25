using UnityEngine;
using System.Collections;

[System.Serializable]
public class EnemySpawnEventParams  {

	[System.NonSerialized]
	public bool m_activated = false;

	public float m_time;
	public GameObject m_enemy;
	public Transform m_spawnPoint;
}
