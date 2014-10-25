using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour {


	public float waveTimer = 0;
	public bool waveHasStarted = false;
	public BasicEnemyScript[] enemiesInWave;
	public Stats[] allTheStats;
	public int enemiesActivated = 0;
	public bool waveComplete = false;
	public bool villagersComplete = false;
	public VillagerScript[] villagersInWave;
	//on trigger enter
	//switch bool
	//call function in update



	// Use this for initialization
	void Start () {
		enemiesInWave = this.GetComponentsInChildren<BasicEnemyScript>(true);
		allTheStats = this.GetComponentsInChildren<Stats>(true);
		villagersInWave = this.GetComponentsInChildren<VillagerScript>();
	}
	
	// Update is called once per frame
	void Update () {
		Wave ();
		CompletionCheck();
	}

	void OnTriggerEnter(Collider other){
		waveHasStarted = true;
	}

	public void Wave(){
		if(waveHasStarted){
			waveTimer += Time.deltaTime;
			villagerCheck();
			if(enemiesActivated < enemiesInWave.Length){
				foreach(BasicEnemyScript script in enemiesInWave){
					if(waveTimer >= script.spawnTime && !script.gameObject.activeInHierarchy){
						script.gameObject.SetActive(true);

						script.sceneHasStarted = true;
						enemiesActivated++;
					}
				}
			}
		}
	}

	void CompletionCheck(){
		int enemiesKilled = 0;
		foreach(Stats stats in allTheStats){
			if(!stats.m_isAlive){
				enemiesKilled++;
			}
		}
		if(enemiesKilled == enemiesInWave.Length){
			waveComplete = true;
		}
	}

	void villagerCheck(){
		int villagersOutOfPlay = 0;
		foreach(VillagerScript villager in villagersInWave){
			if(villager.state == VillagerScript.villagerState.saved){
				villagersOutOfPlay++;
			}
			if(villager.state == VillagerScript.villagerState.dead){
				villagersOutOfPlay++;
			}
		}
		if(villagersOutOfPlay == villagersInWave.Length){
			villagersComplete = true;
		}
	}

}
