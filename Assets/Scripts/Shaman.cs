using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shaman : MonoBehaviour {

    
	private List<GameObject> enemies;					// list of all enemies   
	private List<GameObject> enemySequence;			 	// list of enemies we will cycle through    

	private float maxRangeFromInitPos = 20.0F;			// max range from init pos shaman will go to a enemy    
	private bool hasTarget = false;						// shaman already has a target    
	private GameObject currentTarget;					// pintpoint of current target   
	private float minDistanceBetweenTarget = 5.0F;		// distance when heals/helps target	    
	private int enemyIndex;

	public int healAmount = 3;
    public float castingTime = 5.0f;
    public float currCastTime = 0.0f;

    Animator anim;

	// Use this for initialization
	void Start () {

        anim = GetComponent<Animator>();
        
        anim.SetBool("isDancing", false);

        enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        Debug.Log(enemies.Count);
        enemies.Remove(gameObject);
        Debug.Log(enemies.Count);

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].GetComponent<Shaman>() != null)
            {
                enemies.RemoveAt(i);
                i--;
            }
        }

        enemyIndex = enemies.Count - 1;
	}
	
	// Update is called once per frame
	void Update () {
        
        if(!hasTarget)
        {
            for(int i = 0; i < enemies.Count; i++)
            {
                hasTarget = true;
                anim.SetBool("isDancing", true);

                currentTarget = enemies[enemyIndex];
                enemyIndex--;

                if (enemyIndex <= 0)
                    enemyIndex = enemies.Count - 1;
            }
        }

        if(hasTarget)
        {

            currCastTime += Time.deltaTime;

            if(currCastTime > castingTime)
            {
                currCastTime = 0;
                anim.SetBool("isDancing", false);

                if (currentTarget.GetComponent<Stats>())
                    currentTarget.GetComponent<Stats>().Heal( healAmount );
                RangedAttack rangAtk = currentTarget.GetComponent<RangedAttack>();

                if (rangAtk)
                {
                    rangAtk.LowerShootingLag();
                }

                hasTarget = false;
            }
        }
	}
}

