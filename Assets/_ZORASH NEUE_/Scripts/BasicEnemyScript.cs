using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Stats))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class BasicEnemyScript : MonoBehaviour {

	public enum enemyState{idle, chasing, capturing, blocking, dying}
	public enemyState state;
	public GameObject targetToCapture;
	public GameObject holdingPoint;
	public float captureRange = 2f;
	public bool sceneHasStarted = false;
	public GameObject killPoint;
	public float spawnTime;

	private Stats stats;
	private Animator myAnimator;
	private NavMeshAgent navMeshAgent;
	private VillagerScript myTarget;

	// Use this for initialization
	void Start () {
		if( Network.isClient )
			GetComponent<NetworkInterpolatedTransform>().enabled = true;

		myAnimator = this.GetComponent<Animator>();
		stats = this.GetComponent<Stats>();
		navMeshAgent = this.GetComponent<NavMeshAgent>();
		myTarget = targetToCapture.GetComponent<VillagerScript>();
		//myTarget.enemiesChasingMe++;
	}
	
	// Update is called once per frame
	void Update () {
		if( Network.isServer ) {
			switch(state){
				case enemyState.idle:
					EnemyIdle();
					break;
				case enemyState.chasing:
					EnemyIsChasing();
					break;
				case enemyState.capturing:
					EnemyCapturingVillager();
					break;
				case enemyState.blocking:
					EnemyBlocking();
					break;
				case enemyState.dying: 
					EnemyDie();
					break;
			}
		}
	}

	//plays idle animation, checks for target to chase.
	void EnemyIdle(){
		if(sceneHasStarted){

				this.gameObject.SetActive(true);

				myAnimator.SetBool("Move", true);
				networkView.RPC( "SendBool", RPCMode.Others, "Move", true );
				state = enemyState.chasing;
				//targetToCapture.GetComponent<VillagerScript>().enemiesChasingMe++;
				return;
			//TODO: add spawn timer. add activation. 


		}
		return;
	}

	//follows his target, check if he is in capture range
	void EnemyIsChasing(){
		myAnimator.SetBool("Move", true);
		networkView.RPC( "SendBool", RPCMode.Others, "Move", true );
		navMeshAgent.SetDestination(targetToCapture.transform.position);
		targetToCapture.GetComponent<VillagerScript>().isInDanger = true;
		if(Vector3.Distance(this.transform.position, targetToCapture.transform.position) <= captureRange &&
		   targetToCapture.GetComponent<VillagerScript>().captor == null){
			targetToCapture.GetComponent<VillagerScript>().captured = true;
			state = enemyState.capturing;
			return;
		}
		return;
	}

	//wander around mindlessly inbetween the camera and the villager. 
	//attempt to obscure vision of the zombie holding the villager. 
	void EnemyBlocking(){
		return;
	}

	//trigger's villager's captured bool, starts walking to the deathzone. 
	//check to see if in death zone, if in deathzone, kill villager. 

	void EnemyCapturingVillager(){
		myAnimator.SetBool("PickUp", true);
		becomeVillagerCaptor();
		navMeshAgent.SetDestination(killPoint.transform.position);
		if(Vector3.Distance(this.transform.position, killPoint.transform.position) <= captureRange){
			navMeshAgent.Stop();
			targetToCapture.GetComponent<VillagerScript>().villagerDie();
			state = enemyState.chasing;
			return;
		}
		
		return;
	}

	void TakeDamage(int damage) {
		stats.ApplyDamage(damage);
		
		if (stats.m_currHealth <= 0)
			stats.m_isAlive = false;
			myTarget.enemiesChasingMe--;
			state = enemyState.dying;
	}

	void becomeVillagerCaptor(){
		targetToCapture.GetComponent<VillagerScript>().captor = this.gameObject;
		targetToCapture.GetComponent<VillagerScript>().captured = true;
	}

	void EnemyDie(){
		myAnimator.SetBool("Dead", true);
		networkView.RPC( "SendBool", RPCMode.Others, "Dead", true );
		if(targetToCapture.GetComponent<VillagerScript>().captor == this.gameObject){
			targetToCapture.GetComponent<VillagerScript>().captor = null;
			targetToCapture.GetComponent<VillagerScript>().state = VillagerScript.villagerState.inDanger;
		}
		gameObject.SetActive( false );
		//Destroy(this.gameObject);
	}

	/*
	 * rotationTarget = new Vector3 (Random.RandomRange (transform.position.x + wonderRange, transform.position.x - wonderRange),
                                                      transform.position.y,
                                                      Random.RandomRange (transform.position.z + wonderRange, transform.position.z - wonderRange));
 
                        Quaternion targetRotation = Quaternion.LookRotation(rotationTarget - transform.position);
                        float s = Mathf.Min(lookSpeed * Time.deltaTime, 1);
                        transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(0, targetRotation.y, 0, targetRotation.w), s);
	*/

	[RPC]
	void SendBool( string name, bool value ) {
		myAnimator.SetBool( name, value );
	}
	
	[RPC]
	void SendFloat( string name, float value ) {
		myAnimator.SetFloat( name, value );
	}
}
