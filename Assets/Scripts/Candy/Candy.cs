using UnityEngine;
using System.Collections;

/// <summary>
/// Attached to Candy Object holding all candy
/// </summary>

public class Candy : MonoBehaviour {

    public GameObject feedBack;
    public GameObject player;

    public NetworkInterpolatedTransform Nit;


    private GameObject temp;
    bool hasClicked = false;
    bool spawned = false;

    public float waitTime = 1.0f;
    private float currTime = 0;

    public float destroyDistance = 3.0f;

    void Start() {
        if (Network.isClient)
            Nit.enabled = true;
		player = GameObject.Find( "CameraMotor 2" );
    }

	// Update is called once per frame
	void Update () {

        currTime += Time.deltaTime;
            
        if(currTime > waitTime)
            transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime);

        if(Vector3.Distance(transform.position, player.transform.position) < destroyDistance)
        {
            Destroy(gameObject);

            GameObject.FindGameObjectWithTag("Manager").SendMessage("AddGameStats", 1);
        }
        

	}

    void OnMouseDown()
    {
        hasClicked = true;
    }
}
