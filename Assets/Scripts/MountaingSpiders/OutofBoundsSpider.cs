using UnityEngine;
using System.Collections;

public class OutofBoundsSpider : MonoBehaviour
{

    public Transform outOfBoundsArea;
    public float outOfBoundsDistance = 1.0f;

    private Vector3 initPos;

    public GameObject firstSpider;

    // Use this for initialization
    void Start()
    {
        initPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Vector3.Distance(transform.position, outOfBoundsArea.position) < outOfBoundsDistance)
        {
            gameObject.SetActive(false);
            transform.position = initPos;
            firstSpider.SetActive(true);
        }
    }
}
