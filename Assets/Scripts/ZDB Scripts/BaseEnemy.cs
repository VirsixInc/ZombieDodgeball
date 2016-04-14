using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseEnemy: MonoBehaviour 
{
	public float speed = 1f;
	public int pointValue = 1;
	
	public GameObject pelvis;
	
	public AudioSource hitSound;

	protected bool moving;
	protected bool hasBeenHit = false;
	protected bool attackMode = false;
	protected float deathTime;
	protected Animator animator;
	protected MovementNode prevNode;
	protected MovementNode currNode;
	protected float attackTime = 2f;
	protected bool hasDeathAnim = false;
	protected bool moveSpeedByDistance = true;
	protected Vector3 pelvisPos;
	protected AudioManager audioMan;

	float distToNode;
	float timer = 0.0f;
	bool dead = false;


	protected virtual void Start () 
	{
		deathTime = 1.5f;
		ResetPelvis();
		audioMan = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
	}

	protected virtual void Update () 
	{
		if( dead )
		{
			if( timer >= deathTime )
			{
				Reset();
			}
		}
		else if( moving )
		{
			float lerpPercentage = timer * speed;// / ( distToNode );
			PelvisCheck();
			if( moveSpeedByDistance )
				lerpPercentage /= distToNode;
			
			transform.transform.position = Vector3.Lerp( prevNode.transform.position, currNode.transform.position, lerpPercentage );

			if( lerpPercentage >= 1f )
			{
				GetNextNode();
				MovementCheck();
			}
		}
		else if( attackMode )
		{
			if( timer >= attackTime )
			{
				//slash screen
				Attack();
			}
		}
		timer += Time.deltaTime;
	}

	protected virtual void Reset() 
	{
		moving = false;
		hasBeenHit = false;
		ResetPelvis();
		gameObject.SetActive(false);
	}

	protected virtual void Hit( GameObject hittingObj ) 
	{
		if( hasBeenHit )
			return;
			
		Ball hittingBall = hittingObj.GetComponent<Ball>();
		
		audioMan.PlaySound( audioMan.GetHitSoundName( name ) );

		if( hittingBall.color == PlayerColor.Red ) 
		{
			FloatingTextManager.instance.CreateFloatingText( transform.position, pointValue, Color.red );
		}
		else if( hittingBall.color == PlayerColor.Green)
		{
			FloatingTextManager.instance.CreateFloatingText( transform.position, pointValue, Color.green );
		}
//			else if( hittingBall.color == PlayerColor.Yellow) 
//			{
//				FloatingTextManager.instance.CreateFloatingText( transform.position, pointValue, Color.yellow );
//			}
//			else if( hittingBall.color == PlayerColor.Blue) 
//			{
//				FloatingTextManager.instance.CreateFloatingText( transform.position, pointValue, Color.blue );
//			}

		PlayerManager.AddPoints(hittingBall.color, pointValue);


		hitSound.Play();
		SetKinematic( false );
		hasBeenHit = true;
		if( !hasDeathAnim )
			animator.enabled = false;
//			m_navmeshAgent.Stop();
		dead = true;
		moving = false;
		timer = 0.0f;
		//GetComponent<Animator>().SetBool( "Dead", true );
	}

	protected virtual void MovementCheck()
	{
		if (!attackMode) 
		{
			moving = true;
			transform.LookAt (new Vector3 (currNode.transform.position.x, transform.position.y, currNode.transform.position.z));
		}
	}

	protected virtual void GetNextNode()
	{
		timer = 0.0f;
		prevNode = currNode;
		currNode = currNode.GetNextNode();

		if( currNode == null )
		{
			currNode = prevNode;
			ActivateAttackMode ();
			moving = false;
			return;
		}

		distToNode = Vector3.Distance( prevNode.transform.position, currNode.transform.position );

	}
	
	protected virtual void ActivateAttackMode()
	{
		animator.Play("Attack");
		attackMode = true;
	}
	
	protected virtual void Attack()
	{
		if( !name.Contains("Balloon") )
			audioMan.PlaySound("ZombieAttack");

		if( GameManager.instance.isGamePlaying )
			GameManager.instance.ReduceLives( 1, this );
		Reset();
	}

	protected void ResetTimer()
	{
		timer = 0.0f;
	}

	public virtual void InitialSetup( MovementNode spawnNode )
	{
		currNode = spawnNode;
		transform.position = currNode.transform.position;
		GetNextNode();
		moving = true;
		attackMode = false;
		hasBeenHit = false;
		dead = false;
		if( animator == null )
			animator = GetComponent<Animator> ();
		animator.enabled = true;
		SetKinematic (true);
		ResetPelvis();
		MovementCheck ();
	}

	protected virtual void SetKinematic( bool value ) 
	{
		gameObject.GetComponent<Rigidbody>().isKinematic = value;
	
		Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
		
		foreach( Rigidbody rB in rigidbodies ) 
		{
			rB.isKinematic = value;
			rB.velocity = rB.angularVelocity = Vector3.zero;
		}
	}
	
	protected virtual void HitByExplosion( GameObject hittingObj )
	{
		Hit( hittingObj );
	}
	
	protected virtual void DelayExplosiveForce( object[] parms )
	{
		StartCoroutine( "DelayedExplosiveForce", parms );
	}
	
	public IEnumerator DelayedExplosiveForce( object[] parms )
	{
		yield return new WaitForSeconds( (float)parms[0] );
		
		Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
		
		foreach( Rigidbody rB in rigidbodies ) 
		{
			if( !rB.isKinematic )
				rB.AddExplosionForce( (float)parms[1], (Vector3)parms[2], (float)parms[3] );
		}
	}
	
	void ResetPelvis()
	{
		if( pelvis != null )
		{
			if( pelvisPos == null )
				pelvisPos = pelvis.transform.position;
			pelvis.transform.position = pelvisPos;
		}
	}
	
	void PelvisCheck()
	{
		if( pelvis != null )
		{
			if( Mathf.Abs( pelvis.transform.position.x - pelvisPos.x ) >= 1f )
			{
				ResetPelvis();
			}
		}
	}
	
	public int GetNodeLaneNumber()
	{
		return currNode.slotNumber;
	}
}
