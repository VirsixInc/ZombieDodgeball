using UnityEngine;
using System.Collections;

public class Werewolf : BaseEnemy
{
	float waitTime = 2f;
	float waitTimer;
	float attackDelay;

	bool beganBound;
	
	Quaternion prevRot;
	Quaternion newRot;

	protected override void Start()
	{
		base.Start ();
		speed = 2f;
		attackTime = 1.3f;
		moveSpeedByDistance = false;
		//deathTime = 0f;
	}

	protected override void Update()
	{
		base.Update ();

		if( !moving && !attackMode )
		{
			waitTimer -= Time.deltaTime;

			//if( waitTimer <= 1f )
			transform.rotation = Quaternion.Lerp( newRot, prevRot, waitTimer * 2 );//(waitTime - waitTimer)) / waitTime );

			if( waitTimer <= 0.4f && !beganBound )
			{
				beganBound = true;
				animator.Play("Bound");
				//transform.LookAt( new Vector3( currNode.transform.position.x, transform.position.y, currNode.transform.position.z ) );
			}
			
			if( waitTimer <= 0f )
			{
				moving = true;
				ResetTimer();
			}
		}
		else if( attackMode )
		{
			attackDelay -= Time.deltaTime;
			
			if( attackDelay <= 0f )
			{
				animator.Play("Attack");
			}
		}
	}
	
	protected override void ActivateAttackMode()
	{
		attackMode = true;
		attackDelay = 0.3f;
	}

	protected override void MovementCheck()
	{
		prevRot = transform.rotation;
		newRot = Quaternion.LookRotation(currNode.transform.position - transform.position);
		moving = false;
		waitTimer = waitTime;
		beganBound = false;
	}
}
