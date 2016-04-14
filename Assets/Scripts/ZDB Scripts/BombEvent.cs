using UnityEngine;
using System.Collections;

public class BombEvent : EventSystem
{
	public GameObject explosionParticlesPrefab;
	
	public float explosionRadius;

	AudioManager audioMan;
	
	float collisionDelay = 0.5f;
	float delayTimer = 0.0f;
	
	protected override void Start()
	{
		base.Start();
		audioMan = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
	}
	
	protected override void Update()
	{
		
	}
	
	public override void StartEvent( Ball eBall )
	{
		base.StartEvent( eBall );
		delayTimer = collisionDelay;
	}
	
	void OnCollisionEnter(Collision col) 
	{
		//if( col.transform.tag == "Untagged" )
		//{
		audioMan.PlaySound("Bomb");
		
		Instantiate( explosionParticlesPrefab, transform.position, Quaternion.identity );
		
		Collider[] enemiesHit = Physics.OverlapSphere( transform.position, explosionRadius );
		
		foreach( Collider enemyCol in enemiesHit )
		{
			if( enemyCol.tag == "Enemy" )
			{
				enemyCol.transform.SendMessageUpwards("HitByExplosion", eventBall.gameObject, SendMessageOptions.DontRequireReceiver);
				
				
				object[] parms = new object[4]{0.1f, 500f, transform.position, explosionRadius};
				enemyCol.transform.SendMessageUpwards("DelayExplosiveForce", parms, SendMessageOptions.DontRequireReceiver);
			}
		}
		
		GameObject.Destroy( gameObject );
			
		//}
	}
}
