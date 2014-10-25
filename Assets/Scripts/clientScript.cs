using UnityEngine;
using System.Collections;

public class clientScript : MonoBehaviour {
	
	public bool isAssigned = false;
	public GameObject shotManagerBS;

	void Start () {
		shotManagerBS = GameObject.Find("Manager");
	}
	
	// Update is called once per frame
	void Update () {
	}

  	void OnGUI(){
    	GUI.Label(new Rect(100,5,100,50), Network.player.ToString());
  	}
	public void OSCMessageReceived(OSC.NET.OSCMessage msg){
		ArrayList args = msg.Values;
		shotManagerBS.GetComponent<ShotManager>().NetworkShoot((float)args[0], (1f-(float)args[1]));
		print (args[0] + "    " + args[1]);
	}
}
