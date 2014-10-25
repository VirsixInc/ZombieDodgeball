using UnityEngine;
using System.Collections;

public class Wolf_CandyThief : MonoBehaviour {

    enum ThiefState { Spawn, Move, Steal, Die }

    GameObject[] backSceneWayPoints;


    public GameObject[] targets = new GameObject[4];

	// Use this for initialization
	void Start () {

        backSceneWayPoints = GameObject.FindGameObjectsWithTag("WolfWayPoints");

	}
	
	// Update is called once per frame
	void Update () {



	}
}
