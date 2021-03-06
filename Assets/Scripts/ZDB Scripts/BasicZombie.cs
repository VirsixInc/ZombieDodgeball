﻿using UnityEngine;
using System.Collections;

public class BasicZombie : BaseEnemy
{
	public AnimateTiledTexture biteAnim;
	
	protected override void Start()
	{
		base.Start ();
		biteAnim.Deactivate();
		attackTime = 2f;
	}
	
	protected override void Reset()
	{
		base.Reset();
		biteAnim.Deactivate();
	}
	
	protected override void ActivateAttackMode()
	{
		base.ActivateAttackMode();
		biteAnim.Activate();
	}
}
