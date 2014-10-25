using UnityEngine;
using System.Collections;

public class SpiderSwitch : MonoBehaviour {

    // second spider that is suppose to look like its still the 
    // same spider just rotated a different direction to stay on the wall
    public GameObject secondSpider;

    // distance between the spiders before they switch actives
    public float switchDistance = 1.0f;

    //initial position to retur to
    private Vector3 initPos;    

	// Use this for initialization
	void Start () {

        initPos = transform.position;
        secondSpider.SetActive(false);

	}
	
	// Update is called once per frame
	void Update () {

        if (Vector3.Distance(transform.position, secondSpider.transform.position) < switchDistance)
        {
            secondSpider.SetActive(true);
            gameObject.SetActive(false);
            transform.position = initPos;
        }
	}
}
