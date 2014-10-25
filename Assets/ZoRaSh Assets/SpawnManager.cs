using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour {
	
	HordeWaypoint m_enemySpawnPoint;
	public GameObject[] m_enemyPrefabs;
	public float m_spawnCooldown = 1f;

	private float m_spawnTimer = 0f;

	// Use this for initialization
	void Awake () {
		m_enemySpawnPoint = GameObject.Find( "SpawnPoint" ).GetComponent<HordeWaypoint>();
	}
	
	public void SpawnNewHordeEnemy() {
		int enemyIndex = Random.Range( 0, m_enemyPrefabs.Length );
		BaseHordeEnemy tempEnemy = StaticPool.GetObj( m_enemyPrefabs[enemyIndex] ).GetComponent<BaseHordeEnemy>();
		tempEnemy.transform.position = m_enemySpawnPoint.transform.position;
		tempEnemy.transform.LookAt( new Vector3( Camera.main.transform.position.x, tempEnemy.transform.position.y, Camera.main.transform.position.y ) );

		tempEnemy.StartMoving( m_enemySpawnPoint );	}
}
