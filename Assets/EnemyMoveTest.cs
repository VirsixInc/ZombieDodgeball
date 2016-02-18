using UnityEngine;
using System.Collections;

public class EnemyMoveTest : MonoBehaviour {

	public float speed = 1;

	public GameObject leftWall;
	public GameObject rightWall;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {



		transform.Translate (speed * Time.deltaTime * Vector3.forward);
	}
}
