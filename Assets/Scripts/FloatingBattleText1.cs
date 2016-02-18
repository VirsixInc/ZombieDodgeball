using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextMesh))]

public class FloatingBattleText1 : MonoBehaviour {
	
	//public Transform target;		// Object that this label should follow
	public Vector3 target;
	public Vector3 offset = Vector3.up;	// Units in world space to offset; 1 unit above object by default
	public bool clampToScreen = false;	// If true, label will be visible even if object is off screen
	public float clampBorderSize = .05f;	// How much viewport space to leave at the borders when a label is being clamped
	public bool useMainCamera = true;	// Use the camera tagged MainCamera
	public Camera cameraToUse;	// Only use this if useMainCamera is false
	public float fadeTime = 1.0f;
	public float yGain = 1.0f;	//Meters per second

	bool isActive = false;
	float timer;
	Vector3 yOffset;
	TextMesh tMesh;
 
	void Start () {
		tMesh = GetComponent<TextMesh>();
	}
 
	void Update () {
		if(isActive) {
			yOffset.y += yGain * Time.deltaTime;
			yOffset.z = -1.0f;
			
			transform.position = target + yOffset;

			timer += Time.deltaTime;
			
			if(timer >= fadeTime) {
				isActive = false;
				transform.position = Vector3.one * 1000.0f;
			}
		}
	}
	
	public void ShowText(Vector3 tar, float dmg, Color clr) {
		GetComponent<Renderer>().material.color = clr;
		target = tar;
		isActive = true;
		timer = 0.0f;
		tMesh.text = dmg.ToString();
	}

}
