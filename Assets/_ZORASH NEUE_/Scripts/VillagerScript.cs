using UnityEngine;
using System.Collections;


[RequireComponent(typeof (NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class VillagerScript : MonoBehaviour {

	public enum villagerState {idle, inDanger, hiding, captured, saved, dead};
	public villagerState state;
	public Waypoint safteyZone; //waypoint villager moves towards once scene starts/is in danger
	public bool isInDanger = false; 
	public bool captured = false;
	public bool isSafe = false;
	public int enemiesChasingMe = 0;

	public float safetyZoneRange = 1f; //the rang at which to switch on "hiding"

	public GameObject captor;

	public NavMeshAgent navMeshAgent;
	public Animator myAnimator;

	// Use this for initialization
	void Start () {
		if( Network.isClient )
			GetComponent<NetworkInterpolatedTransform>().enabled = true;

		navMeshAgent = this.GetComponent<NavMeshAgent>();
		myAnimator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if( Network.isServer ) {
			switch(state){
				case villagerState.idle:
					VillagerIdle();
					break;
				case villagerState.inDanger:
					VillagerInDanger();
					break;
				case villagerState.hiding:
					VillagerHiding();
					break;
				case villagerState.captured:
					VillagerCaptured();
					break;
				case villagerState.saved:
					break;
				case villagerState.dead:
					break;
			}
		}
	}

	void VillagerIdle(){
		if(isInDanger){
			state = villagerState.inDanger;
			return;
		}
		else{
			return;
		}

	}

	void VillagerInDanger(){
		myAnimator.SetBool("Run", true);
		networkView.RPC( "SendBool", RPCMode.Others, "Run", true );
		navMeshAgent.SetDestination(safteyZone.transform.position);
		if(Vector3.Distance(this.transform.position, safteyZone.transform.position)<=safetyZoneRange){
			myAnimator.SetBool("Run", false);
			networkView.RPC( "SendBool", RPCMode.Others, "Run", false );
			state = villagerState.hiding;
			return;
		}
		if(captured){
			myAnimator.SetBool("Run", false);
			networkView.RPC( "SendBool", RPCMode.Others, "Run", false );
			state = villagerState.captured;
			return;
		}
		SafetyCheck();
		if(isSafe){
			myAnimator.SetBool("Run", false);
			networkView.RPC( "SendBool", RPCMode.Others, "Run", false );
			enemiesChasingMe--;
			state = villagerState.saved;
			return;
		}
	}
	//Stop villager moving. check to see if villager was capture or saved, shange state accordingly. 
	void VillagerHiding(){
		myAnimator.SetBool("Cowering", true);
		networkView.RPC( "SendBool", RPCMode.Others, "Cowering", true );
		navMeshAgent.Stop();
		SafetyCheck();
		if(captured){
			myAnimator.SetBool("Cowering", false);
			networkView.RPC( "SendBool", RPCMode.Others, "Cowering", false );
			state = villagerState.captured;
			return;
		}

		else if(isSafe){
			myAnimator.SetBool("Cowering", false);
			networkView.RPC( "SendBool", RPCMode.Others, "Cowering", false );
			enemiesChasingMe--;
			state = villagerState.saved;
			return;
		}
		else{
			return;
		}
	}
	//villager is moved to captured position and rotated. checks to see if he is no longer captured
	//checks to see if he is safe. 
	void VillagerCaptured(){
		if(captor != null){
			myAnimator.SetBool("PickedUp", true);
			networkView.RPC( "SendBool", RPCMode.Others, "PickedUp", true );
			this.transform.position = captor.GetComponent<BasicEnemyScript>().holdingPoint.transform.position;
		}
		if(captor == null){
			myAnimator.SetBool("Dropped", true);
			networkView.RPC( "SendBool", RPCMode.Others, "Dropped", true );
			captured = false;
			state = villagerState.inDanger;
		}
		if(!captured && !isSafe){
			myAnimator.SetBool("PickedUp", false);
			networkView.RPC( "SendBool", RPCMode.Others, "PickedUp", false );
			myAnimator.SetBool("Dropped", true);
			networkView.RPC( "SendBool", RPCMode.Others, "Dropped", true );
			state = villagerState.inDanger;
			return;
		}
		SafetyCheck();
		if(isSafe){
			myAnimator.SetBool("PickedUp", false);
			networkView.RPC( "SendBool", RPCMode.Others, "PickedUp", false );
			state = villagerState.saved;
			enemiesChasingMe--;
			return;
		}
		//TODO: if entered deathzone: die. 
	}

	void SafetyCheck(){
		if(enemiesChasingMe == 0){
			isSafe = true;
		}
	}

	public void villagerDie(){
		enemiesChasingMe = 0;
		state = villagerState.dead;
		gameObject.SetActive( false );
//		Destroy(this.gameObject);
	}

	[RPC]
	void SendBool( string name, bool value ) {
		myAnimator.SetBool( name, value );
	}

	[RPC]
	void SendFloat( string name, float value ) {
		myAnimator.SetFloat( name, value );
	}
}
