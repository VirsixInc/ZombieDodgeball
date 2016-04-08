using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseEnemy: MonoBehaviour 
{
	public float speed = 1f;
	public int pointValue = 1;

	protected bool moving;
	protected bool hasBeenHit = false;
	protected bool attackMode = false;
	protected float deathTime;
	protected Animator animator;
	protected MovementNode prevNode;
	protected MovementNode currNode;
	protected float attackTime = 2f;
	protected bool hasDeathAnim = false;

	float distToNode;
	float timer = 0.0f;
	bool dead = false;


	protected virtual void Start () 
	{
		deathTime = 1.5f;
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
			float lerpPercentage = timer * speed / ( distToNode );

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
		gameObject.SetActive(false);
	}

	protected virtual void Hit( GameObject hittingObj ) 
	{
		if( !hasBeenHit ) 
		{
			Ball hittingBall = hittingObj.GetComponent<Ball>();


			if( hittingBall.color == PlayerColor.Red ) 
			{
				FloatingTextManager.instance.CreateFloatingText( transform.position, pointValue, Color.red );
			}
			else if( hittingBall.color == PlayerColor.Green)
			{
				FloatingTextManager.instance.CreateFloatingText( transform.position, pointValue, Color.green );
			}
			else if( hittingBall.color == PlayerColor.Yellow) 
			{
				FloatingTextManager.instance.CreateFloatingText( transform.position, pointValue, Color.yellow );
			}
			else if( hittingBall.color == PlayerColor.Blue) 
			{
				FloatingTextManager.instance.CreateFloatingText( transform.position, pointValue, Color.blue );
			}

			PlayerManager.AddPoints(hittingBall.color, pointValue);


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
		if( GameManager.instance.isGamePlaying )
			GameManager.instance.ReduceLives( 1, this );
		Reset();
	}

	protected void ResetTimer()
	{
		timer = 0.0f;
	}

	public void InitialSetup( MovementNode spawnNode )
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
		MovementCheck ();
	}

	protected virtual void SetKinematic( bool value ) 
	{
		Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
		
		foreach( Rigidbody rB in rigidbodies ) 
		{
			rB.isKinematic = value;
		}
	}
	
	public int GetNodeLaneNumber()
	{
		return currNode.slotNumber;
	}
}
