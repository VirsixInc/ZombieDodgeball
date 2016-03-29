using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour 
{
	public MovementNode spawnNode;
	public MovementNode flyingSpawnNode;

	HordeWaypoint m_enemySpawnPoint;
	public GameObject[] m_enemyPrefabs;

	public int baseNumBasicZombies;
	public int baseNumFlyingZombies;
	public int baseNumWerewolves;
	public float baseSpawnRate;

	int bzLeft;
	int fzLeft;
	int wwLeft;
	float spawnRate;
	float spawnTimer = 0.0f;

	bool spawning = false;
//	public float m_spawnCooldown = 1f;
//
//	private float m_spawnTimer = 0f;


	void Update()
	{
		if( !spawning )
			return;

		spawnTimer += Time.deltaTime;

		if( spawnTimer >= spawnRate )
		{
			spawnTimer = 0.0f;
			int rand = Random.Range( 1, bzLeft + fzLeft + wwLeft );

			if( rand <= bzLeft )
			{
				bzLeft--;
				SpawnNewHordeEnemy( 0 );
			}
			else if( rand <= bzLeft + fzLeft )
			{
				fzLeft--;
				SpawnNewHordeEnemy( 2 );
			}
			else
			{
				wwLeft--;
				SpawnNewHordeEnemy( 1 );
			}

			RoundOverCheck();
		}
	}

	public void NewSpawnRound( int round )
	{
		bzLeft = baseNumBasicZombies * round;
		fzLeft = baseNumFlyingZombies * round;
		wwLeft = baseNumWerewolves * round;
		spawnRate = baseSpawnRate / round;
		spawning = true;
		spawnTimer = 0.0f;
	}

	public bool WaveOverCheck()
	{
		if( spawning )
			return false;
		
		bool waveOver = true;

		foreach(GameObject go in m_enemyPrefabs)
		{
			waveOver = StaticPool.AllEnemiesDeadCheck( go ) && waveOver;
		}

		if( waveOver )
			Debug.Log("Wave is over in SM");

		return waveOver;
	}

	void RoundOverCheck()
	{
		if( bzLeft + fzLeft + wwLeft <= 0 )
		{
			spawning = false;
			Debug.Log("SM is done spwaning");
			//GameManager.instance.RoundOver();
		}
	}
	
	void SpawnNewHordeEnemy( int enemyIndex ) 
	{
		BaseEnemy enemy = StaticPool.GetObj( m_enemyPrefabs[enemyIndex] ).GetComponent<BaseEnemy>();
		if( enemyIndex == 2 )
			enemy.InitialSetup( flyingSpawnNode );
		else
			enemy.InitialSetup( spawnNode );
	}
}
