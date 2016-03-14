using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseEnemy: MonoBehaviour 
{
	public float speed = 1f;
	public int pointValue = 1;

	protected bool moving;
	protected bool hasBeenHit = false;

	MovementNode prevNode;
	MovementNode currNode;
	float distToNode;
	float timer = 0.0f;
	float deathTime = 2f;
	bool dead = false;


	void Start () 
	{
		
	}

	protected virtual void Update () 
	{
		if( dead )
		{
			if( timer >= 2f )
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
				MovementCheck();
				GetNextNode();
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

	protected  void Hit( GameObject hittingObj ) 
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


//			SetKinematic( false );
			hasBeenHit = true;
//			m_animator.enabled = false;
//			m_navmeshAgent.Stop();
			dead = true;
			moving = false;
			timer = 0.0f;
			GetComponent<Animator>().SetBool( "Dead", true );
		}
	}

	protected virtual void MovementCheck()
	{
		moving = true;
	}

	protected void GetNextNode()
	{
		timer = 0.0f;
		prevNode = currNode;
		currNode = currNode.GetNextNode();

		if( currNode == null )
		{
			//decrease life
			moving = false;
			return;
		}

		distToNode = Vector3.Distance( prevNode.transform.position, currNode.transform.position );

		transform.LookAt( new Vector3( currNode.transform.position.x, transform.position.y, currNode.transform.position.z ) );
	}

	public void InitialSetup( MovementNode spawnNode )
	{
		currNode = spawnNode;
		transform.position = currNode.transform.position;
		GetNextNode();
		moving = true;
		hasBeenHit = false;
		dead = false;
	}
}
