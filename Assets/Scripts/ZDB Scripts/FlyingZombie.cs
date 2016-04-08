using UnityEngine;
using System.Collections;

public class FlyingZombie : BaseEnemy 
{
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
	
	protected override void Attack()
	{
		attackMode = true;
	}
}