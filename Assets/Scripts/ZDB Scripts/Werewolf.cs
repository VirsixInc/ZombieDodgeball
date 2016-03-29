using UnityEngine;
using System.Collections;

public class Werewolf : BaseEnemy
{
	float waitTime = 2f;
	float waitTimer;

	bool beganBound;

	protected override void Start()
	{
		base.Start ();
		speed = 10f;
		//deathTime = 0f;
	}

	protected override void Update()
	{
		base.Update ();

		if( !moving && !attackMode )
		{
			waitTimer -= Time.deltaTime;

			if( waitTimer <= 0.4f && !beganBound )
			{
				beganBound = true;
				animator.Play("Bound");
				transform.LookAt( new Vector3( currNode.transform.position.x, transform.position.y, currNode.transform.position.z ) );
			}

			if( waitTimer <= 0f )
			{
				moving = true;
				ResetTimer();
			}
		}
	}

	protected override void MovementCheck()
	{
		moving = false;
		waitTimer = waitTime;
		beganBound = false;
	}
}
