//ABOUT: This script makes the basic enemys inside a particular parent object go. 
//just drag in a parent object that contains any number of basic enemys, 
//then call the MakeAllBasicEnemysGo function in a function event. should work! 

using UnityEngine;
using System.Collections;

public class BasicEnemyGoScript : MonoBehaviour {

	public GameObject basicEnemyParentObject;
	public BasicEnemyScript[] enemys;

	// Use this for initialization
	void Start () {
		enemys = basicEnemyParentObject.GetComponentsInChildren<BasicEnemyScript>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void MakeAllBasicEnemiesGo(){
		foreach(BasicEnemyScript script in enemys){
			script.sceneHasStarted = true;
		}
	}
}
