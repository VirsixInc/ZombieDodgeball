using UnityEngine;
using System.Collections;

public class MassAggroEventTrigger : MonoBehaviour {

	public GameObject m_zombieMobParentObj;
	private BasicEnemyScript[] m_zombies;

	void Start() {
		if (Network.isClient) {
			gameObject.SetActive( false );
			return;
		}

		m_zombies = m_zombieMobParentObj.GetComponentsInChildren<BasicEnemyScript> ();
	}

	void OnTriggerEnter( Collider col ) {
		if (col.tag == "Player") {
			AggroZombies();
			gameObject.SetActive( false );
		}
	}

	void AggroZombies() {
		foreach (BasicEnemyScript zombie in m_zombies) {
			//zombie.SetAggroTrue();
			//zombie.networkView.RPC( "SetAggroTrue", RPCMode.Others );
		}
	}
}
