using UnityEngine;
using System.Collections;

public class FlyingZombie : BaseEnemy 
{
	public Transform bombTransform;
	public GameObject explosionParticlesPrefab;

	protected override void Start()
	{
		base.Start();
		hasDeathAnim = true;
	}

	protected override void Hit( GameObject hittingObj ) 
	{
		base.Hit( hittingObj );
		animator.Play("Death");
	}
	
	protected override void SetKinematic( bool value ) 
	{
	}
	
	protected override void ActivateAttackMode()
	{
		attackMode = true;
	}
	
	protected override void Attack()
	{
		base.Attack();
		Instantiate( explosionParticlesPrefab, bombTransform.position, Quaternion.identity );
	}
}