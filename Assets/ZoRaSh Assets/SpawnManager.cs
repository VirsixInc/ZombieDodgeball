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
		float rand = Random.Range( 0f, 1f );
		int enemyIndex = 0;
			
		if( rand < 0.5f ) {
			enemyIndex = 0;
		} else if ( rand >= 0.5f && rand < 0.95f ) {
			enemyIndex = 2;
		} else if ( rand >= 0.95f ) {
			enemyIndex = 1;
		}

		BaseHordeEnemy tempEnemy = StaticPool.GetObj( m_enemyPrefabs[enemyIndex] ).GetComponent<BaseHordeEnemy>();
		tempEnemy.transform.position = m_enemySpawnPoint.transform.position;
		tempEnemy.transform.LookAt( new Vector3( Camera.main.transform.position.x, tempEnemy.transform.position.y, Camera.main.transform.position.y ) );

		tempEnemy.StartMoving( m_enemySpawnPoint );	}
}
