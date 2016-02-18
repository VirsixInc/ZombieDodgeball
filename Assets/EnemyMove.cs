using UnityEngine;
using System.Collections;

public class EnemyMove : MonoBehaviour {

	float speed = 1;
	Vector3 destination;
	bool needsDest;

	public GameObject leftWall;
	public GameObject rightWall;

	private float startTime;
	private float journeyLength;
	// Use this for initialization
	void Start () {
		needsDest = true;
		leftWall = GameObject.Find ("LeftWall");
		rightWall = GameObject.Find ("RightWall");

	}
		
	void Update () {
		if (needsDest) {
			destination = getNewDest ();
			needsDest = false;
		}

		Vector3 newPos;
		newPos = transform.position;
		newPos = transform.position + ((destination - transform.position) * (speed*Time.deltaTime));

		transform.position = newPos;
		//transform.LookAt (destination);
		//print (Vector3.Normalize(destination-transform.position));
		if (Vector3.Distance(transform.position,destination) < 0.1f){
			needsDest = true;
			return;
		}
		/*
		if (colliding) {
			needsDest = true;
		} else {
			needsDest = false;
		}

		if (needsDest) {
			destination = getNewDest ();
		}

		Vector3 direction = destination - transform.position;
		float angle = Vector3.Angle (direction, transform.forward);
		transform.eulerAngles = new Vector3 (0,angle,0);
		transform.Translate (speed * direction * Time.deltaTime);

		Debug.Log (direction);
//		float percent = Vector3.Distance(transform.position, destination);
//		Debug.Log (percent);

//		if (percent <= .10) {
//			needsDest = true;
//			Debug.Log ("Worked");
//		} else {
//			needsDest = false;
//		}
*/
	}
	Vector3 getNewDest(bool useBoth = true, int dir = 0){
		if (useBoth = true) {
			int maxMov = 5;
			Vector3 tarVec = transform.position;
			tarVec.x += Random.Range (maxMov, maxMov);
			print (tarVec.x);
			tarVec.z -= maxMov;
			return tarVec;
		} else {
			return Vector3.one;
		}
	}
}
