using UnityEngine;
using System.Collections;

/// <summary>
/// Character walks to a point and returns to initial spot 
/// to continue walking
/// </summary>

public class RepeatingSpider : MonoBehaviour {

    public Transform returnPoint;

    private Vector3 initPos;

    public float maxReturnPointDistance = 1.0f;

	// Use this for initialization
	void Start () {

        initPos = transform.position;
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Vector3.Distance(transform.position, returnPoint.position) < maxReturnPointDistance)
            transform.position = initPos;
	
	}
}
