﻿using UnityEngine;
using System.Collections;

public class HealthContainer : MonoBehaviour 
{
	public GameObject fullContainer;
	public GameObject shatteredContainer;
	
	public GameObject shatterPrefab;
	
	public ParticleSystem shatterParticles;
	
	public Transform explosionPoint;
	
	public float explosionForce;
	public float explosionRadius;
	
	public float randomExplosionVariant;

	AudioManager audioMan;
	
	Animator animator;
	
	bool shattered;
	
	void Start()
	{
		Reset ();
		animator = gameObject.GetComponent<Animator>();
		audioMan = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
	}
	
	public void Shatter()
	{
		fullContainer.SetActive(false);
		shatteredContainer.SetActive(true);
		shattered = true;
		
		audioMan.PlaySound("HeartContainerShatter");
		
		GameObject go = (GameObject)Instantiate( shatterPrefab, shatteredContainer.transform.position, shatteredContainer.transform.rotation );
		
		float randomExplosiveForce = Random.Range( explosionForce - ( randomExplosionVariant / 2 ), explosionForce + ( randomExplosionVariant / 2 ) );
		
		foreach( Transform child in go.transform )
			child.GetComponent<Rigidbody>().AddExplosionForce( randomExplosiveForce, explosionPoint.position, explosionRadius );
			
		shatterParticles.Play();
		animator.Play("HealthContainer_Hit");
	}
	
	public void Reset()
	{
		fullContainer.SetActive(true);
		shatteredContainer.SetActive(false);
		shattered = false;
		audioMan = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
	}
	
	public bool IsShattered()
	{
		return shattered;
	}
}
