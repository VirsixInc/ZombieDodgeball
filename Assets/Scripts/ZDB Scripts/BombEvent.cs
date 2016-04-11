using UnityEngine;
using System.Collections;

public class BombEvent : EventSystem
{
	public GameObject explosionParticlesPrefab;
	
	float collisionDelay = 0.5f;
	float delayTimer = 0.0f;
	
	Collider coll;
	
	protected override void Start()
	{
		base.Start();
		coll = gameObject.GetComponent<Collider>();
	}
	
	protected override void Update()
	{
		if( !coll.enabled )
		{
			delayTimer -= Time.deltaTime;
			if( delayTimer <= 0.0f )
				coll.enabled = true;
		}
	}
	
	public override void StartEvent( Ball eBall )
	{
		base.StartEvent( eBall );
		if( coll == null )
			coll = gameObject.GetComponent<Collider>();
		coll.enabled = false;
		delayTimer = collisionDelay;
	}
	
	void OnCollisionEnter(Collision col) 
	{
		if( col.transform.tag == "Untagged" )
		{
			Instantiate( explosionParticlesPrefab, transform.position, Quaternion.identity );
			
			Collider[] enemiesHit = Physics.OverlapSphere( transform.position, 5f );
			
			foreach( Collider enemyCol in enemiesHit )
			{
				Debug.Log ("Hit something");
				if( enemyCol.tag == "Enemy" )
				{
					Debug.Log("It was an enemy!");
					enemyCol.transform.SendMessageUpwards("Hit", eventBall.gameObject, SendMessageOptions.DontRequireReceiver);
				}
			}
			
			GameObject.Destroy(this.gameObject);
		}
	}
}
