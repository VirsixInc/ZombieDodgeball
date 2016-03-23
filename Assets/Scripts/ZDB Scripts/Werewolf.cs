using UnityEngine;
using System.Collections;

public class Werewolf : BaseEnemy
{
	float waitTime = 2f;
	float waitTimer;

	protected override void Start()
	{
		base.Start ();
		speed = 10f;
		//deathTime = 0f;
	}

	protected override void Update()
	{
		base.Update ();

		if( !moving )
		{
			waitTimer -= Time.deltaTime;

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
	}
}
