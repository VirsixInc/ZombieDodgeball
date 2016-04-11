using UnityEngine;
using System.Collections;

public class FlyingZombie : BaseEnemy 
{
	public Transform bombTransform;
	public GameObject explosionParticlesPrefab;
	public GameObject bombPrefab;

	protected override void Start()
	{
		base.Start();
		hasDeathAnim = true;
	}

	protected override void Hit( GameObject hittingObj ) 
	{
		if( hasBeenHit )
			return;
			
		base.Hit( hittingObj );
		animator.Play("Death");
		BombEvent be = ((GameObject)Instantiate( bombPrefab, bombTransform.position, bombTransform.rotation )).GetComponent<BombEvent>();
		be.StartEvent( hittingObj.GetComponent<Ball>() );
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