using UnityEngine;
using System.Collections;

public class ProjectileMovement : MonoBehaviour {

    bool isActive = false;

    // target to shoot at
    private GameObject target;

    // object that will obstruct the camera view.
    public GameObject feedback;
    private GameObject tempFeedBack;

    public float shootSpeed = 10.0f;

    public float videoAbstractionPixelOffset = 256;

	// Use this for initialization
	void Start () {

        target = GameObject.FindGameObjectWithTag("Player");
        tempFeedBack = (GameObject)Instantiate(feedback);
	}
	
	// Update is called once per frame
	void Update () {

        // Early out if we don't have a target
        if (!target)
            return;

        if(isActive)
        {
            Vector3 positionOnScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2 + videoAbstractionPixelOffset, Screen.height / 2, Camera.main.nearClipPlane + 1));

            transform.LookAt(target.transform.position);

            if (Vector3.Distance(positionOnScreen, transform.position) > 5.0F)
            {
                GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(0, shootSpeed / 1.5F, shootSpeed));
            }
            else if (Vector3.Distance(positionOnScreen, transform.position) < 3.0F)
            {
                if (tempFeedBack)
                {
                    isActive = false;
                    tempFeedBack.transform.position = positionOnScreen;
                    tempFeedBack.transform.parent = target.transform;
                    Vector3 positionOffScreen = new Vector3(0, -100, 0);

                    transform.position = positionOffScreen;
                }
                else
                    Debug.Log("ERROR IM NULL!!!");
            }
        }
	}

    public void Activate()
    {
        isActive = true;
    }
}
