using UnityEngine;
using System.Collections;

/// <summary>
/// Attach to enemy object carrying collider/NetworkView
/// </summary>

public class DropCandy : MonoBehaviour {

    /// <summary>
    /// this script need to be attached to the object that has the Stats Script attached
    /// </summary>

    public GameObject[] candy;
    Stats m_stats;
    int numOfCandyToSpawn;
    bool hasSpawned = false;

    public float dropRange = 5.0f;

	// Use this for initialization
	void Start () {

        m_stats = GetComponent<Stats>();

		int health = m_stats.m_currHealth;

        if (health <= 3)
            numOfCandyToSpawn = 2;
        else if (health > 3 && health < 6)
            numOfCandyToSpawn = 3;
        else
            numOfCandyToSpawn = 3;
	}
	
	// Update is called once per frame
	void Update () {

        if (m_stats.m_currHealth <= 0)
        {
            if(hasSpawned == false)
            {
                Spawn();
            }
        }
	}

    void Spawn()
    {
        hasSpawned = true;

        // spawn from middle of object, so the candy can fall into a pile below
        for (int i = 0; i < numOfCandyToSpawn; i++)
        {
            Vector3 spawnPoint = new Vector3(Random.Range(transform.position.x - dropRange, transform.position.x + dropRange),
                                       Random.Range(transform.position.y + 1, transform.position.y + (dropRange)),
                                       Random.Range(transform.position.z - dropRange, transform.position.z + dropRange));
            Network.Instantiate(candy[i], spawnPoint, Quaternion.identity, 0);
        }
    }    
}
