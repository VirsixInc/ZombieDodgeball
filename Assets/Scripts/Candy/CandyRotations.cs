using UnityEngine;
using System.Collections;

public class CandyRotations : MonoBehaviour {

    /// <summary>
    /// Attached to all candy
    /// </summary>
    

    public float tiltAngle = 5.0f;
    public float smoothRotation = 2.0f;
    private float tiltAroundY = 0.0f;
    
	// Update is called once per frame
	void Update () {

    
        tiltAroundY += tiltAngle;

        Quaternion targetRotation = Quaternion.Euler(0, tiltAroundY, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothRotation);
	
	}
}
