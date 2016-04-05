using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenEffectManager : MonoBehaviour 
{
	public List<ScreenEffect> screenDamageEffects = new List<ScreenEffect>();
	public List<ScreenEffect> zombieDamageEffects = new List<ScreenEffect>();
	public List<ScreenEffect> flyingZombieDamageEffects = new List<ScreenEffect>();
	public List<ScreenEffect> werewolfDamageEffects = new List<ScreenEffect>();
	
	void Start()
	{
		ResetAllEffects();
	}
	
	void Update()
	{
	}
	
	
	public void damageScreen()
	{
		foreach( ScreenEffect se in screenDamageEffects )
		{
			se.Activate();
		}
	}
	
	public void ZombieHitEffect()
	{
		foreach( ScreenEffect se in zombieDamageEffects )
		{
			se.Activate();
		}
	}
	
	public void FlyingZombieHitEffect()
	{
		foreach( ScreenEffect se in flyingZombieDamageEffects )
		{
			se.Activate();
		}
	}
	
	public void WerewolfHitEffect()
	{
		Debug.Log("werewolf list start");
		foreach( ScreenEffect se in werewolfDamageEffects )
		{
			Debug.Log("werewolf effect found");
			se.Activate();
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
