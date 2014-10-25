using UnityEngine;
using System.Collections;

public class GravitySpider : MonoBehaviour {

    private Vector3 moveDirection = Vector3.zero;
    CharacterController controller;
    public float gravity = -10.0f;

	// Use this for initialization
	void Start () {

        controller = GetComponent<CharacterController>();
        moveDirection.y = gravity;
	}
	
	// Update is called once per frame
	void Update () {

        controller.Move(moveDirection * Time.deltaTime);
	    
	}
}
