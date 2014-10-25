using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]

public class SpiderSimpleMove : MonoBehaviour {

    private Vector3 moveDirection = Vector3.zero;
    CharacterController controller;

    public float speed = 2.0f;

	// Use this for initialization
	void Start () {

        controller = GetComponent<CharacterController>();
	
	}
	
	// Update is called once per frame
	void Update () {

        moveDirection = transform.TransformDirection(Vector3.forward);
        controller.Move(moveDirection * Time.deltaTime * speed);

	}
}
