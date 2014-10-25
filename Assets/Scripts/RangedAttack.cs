using UnityEngine;
using System.Collections;

public class RangedAttack : MonoBehaviour {
    
    ///Summary
    /// Ranged enemy that will throw a projectile
    /// this projectile will attach itself to the 
    /// camera and abstract all view and shooting until you shoot if off
    ///Summary

    // target to shoot at
    private GameObject target;

    // placeholder position for projectile to be at when it is called
    public Transform projectilePosition;

    // projectile to shoot
    public GameObject projectile;
    private GameObject[] projectilesToShoot;

    // timers to control shooting
    public float lagTimeBetweenShooting = 3.0F;
    public float currTimeAfterShooting = 0.0F;

    // timers for projectile to fall off
    public float maxTimeAttachedToCamera = 3.0F;
    private float currTimeAttachedToCamera = 0.0F;

    // bool to trigger this enemy to start attacking
    public bool startAttacking = false;

    public float amountShamanWillLowerShootingLag = 2.0F;
    private bool shootLagHasBeenLoweredBefore = false;

	// Use this for initialization
	void Start () {

        // initialize array of projectiles/player target
        projectilesToShoot = new GameObject[3];
        target = GameObject.FindGameObjectWithTag("Player");

        // out of range position of the static pool of projectiles
        Vector3 pos = new Vector3(0, -500, 0);
        //shoving projectile into static pool of projectiles 
        for (int i = 0; i < projectilesToShoot.Length; i++ )
        {
            projectilesToShoot[i] = (GameObject)Instantiate(projectile, pos, Quaternion.identity);
        }
	}
	
	// Update is called once per frame
	void Update () {

        transform.LookAt(target.transform.position);

        // Early out if we don't have a target
        if (!target)
            return;
	
        if(startAttacking)
        {
            currTimeAfterShooting += Time.deltaTime;

            if(currTimeAfterShooting > lagTimeBetweenShooting)
            {
                currTimeAfterShooting = 0.0F;

                // find a projectile that is not being used
                for(int i = 0; i < projectilesToShoot.Length; i++)
                {
                    if(projectilesToShoot[i].transform.position.y < 0)
                    {
                        Shoot(projectilesToShoot[i]);

                        break;
                    }
                }
            }
        }
	}

    public void Shoot(GameObject projectile)
    {
        projectile.transform.position = projectilePosition.position;
        projectile.GetComponent<ProjectileMovement>().Activate();
    }

    public void TurnOnAttack()
    {
        startAttacking = true;
    }

    public void LowerShootingLag()
    {
        if(!shootLagHasBeenLoweredBefore)
        {
            lagTimeBetweenShooting -= amountShamanWillLowerShootingLag;
            shootLagHasBeenLoweredBefore = true;
        }
    }
}