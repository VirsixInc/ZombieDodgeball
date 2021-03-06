﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenEffectManager : MonoBehaviour 
{
	public List<ScreenEffect> screenDamageEffects = new List<ScreenEffect>();
	public List<ScreenEffect> zombieDamageEffects = new List<ScreenEffect>();
	public List<ScreenEffect> flyingZombieDamageEffects = new List<ScreenEffect>();
	public List<ScreenEffect> werewolfDamageEffects = new List<ScreenEffect>();
	
	public Transform leftPos;
	public Transform centerPos;
	public Transform rightPos;
	
	public Transform worldLeftPos;
	public Transform worldCenterPos;
	public Transform worldRightPos;
	
	Vector3 imagePos;
	Vector3 worldImagePos;
	
	void Start()
	{
		ResetAllEffects();
	}
	
	void Update()
	{
	}
	
	
	public void damageScreen( BaseEnemy enemy )
	{
		foreach( ScreenEffect se in screenDamageEffects )
		{
			se.Activate();
		}
		
		switch( enemy.GetNodeLaneNumber() )
		{
		case 1:
			imagePos = leftPos.position;
			worldImagePos = worldLeftPos.position;
			break;
		case 2:
			imagePos = centerPos.position;
			worldImagePos = worldCenterPos.position;
			break;
		case 3:
			imagePos = rightPos.position;
			worldImagePos = worldRightPos.position;
			break;
		default:
			imagePos = centerPos.position;
			worldImagePos = worldCenterPos.position;
			break;
		}
	}
	
	public void ZombieHitEffect()
	{
		foreach( ScreenEffect se in zombieDamageEffects )
		{
//			se.Activate();
//			worldImagePos.y = se.transform.position.y;
//			se.transform.position = worldImagePos;
			se.Activate();
			imagePos.y = se.transform.position.y;
			se.transform.position = imagePos;
		}
	}
	
	public void FlyingZombieHitEffect()
	{
		foreach( ScreenEffect se in flyingZombieDamageEffects )
		{
			se.Activate();
			imagePos.y = se.transform.position.y;
			se.transform.position = imagePos;
		}
	}
	
	public void WerewolfHitEffect()
	{
		Debug.Log("werewolf list start");
		foreach( ScreenEffect se in werewolfDamageEffects )
		{
			Debug.Log("werewolf effect found");
			se.Activate();
			imagePos.y = se.transform.position.y;
			se.transform.position = imagePos;
		}
	}
	
	void ResetAllEffects()
	{
		foreach( ScreenEffect se in screenDamageEffects )
		{
			se.gameObject.SetActive(false);
		}
		foreach( ScreenEffect se in zombieDamageEffects )
		{
			se.gameObject.SetActive(false);
		}
		foreach( ScreenEffect se in flyingZombieDamageEffects )
		{
			se.gameObject.SetActive(false);
		}
		foreach( ScreenEffect se in werewolfDamageEffects )
		{
			se.gameObject.SetActive(false);
		}
	}
}
