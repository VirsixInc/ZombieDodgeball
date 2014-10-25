using UnityEngine;
using System.Collections;

public class DebugCamera : MonoBehaviour {
	public bool isMoving = false;
	public Waypoint waypoint;
	public NavMeshAgent myNavMeshAgent;
	private bool waiting = false;
	public WaveManager currentWave;

	// Use this for initialization
	void Start () {
		myNavMeshAgent = this.GetComponent<NavMeshAgent>();

	}
	
	// Update is called once per frame
	void Update () {
		if(isMoving){
			Move();
			//WaveCheck();
			NextWaypointCheck();
		}
		else if(waiting){
			WaveCompletionCheck();
		}
	}

	void Move(){
		myNavMeshAgent.SetDestination(waypoint.transform.position);
	}

	void NextWaypointCheck(){
		if(Vector3.Distance(this.transform.position, waypoint.transform.position) <= 2f){
			if(WaveCheck()){
				return;
			}
			waypoint = waypoint.m_next;

		}
	}

	bool WaveCheck(){
		if(waypoint.gameObject.GetComponent<WaveManager>() != null){
			myNavMeshAgent.Stop();
			isMoving = false;
			waiting = true;
			currentWave = waypoint.GetComponent<WaveManager>();
			waypoint = waypoint.m_next;
			return true;
		}
		else{
			return false;
		}
	}

	void WaveCompletionCheck(){
		if(currentWave.villagersComplete){
			isMoving = true;
		}

		if(currentWave.waveComplete && currentWave.villagersInWave.Length == 0){
			isMoving = true;
		}
	}
}
