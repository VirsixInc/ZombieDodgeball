using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovementNode : MonoBehaviour 
{
	public List<MovementNode> nextNodes = new List<MovementNode>();

	public bool flyingNode;

	void Start () 
	{
	
	}

	public MovementNode GetNextNode()
	{
		if( nextNodes.Count <= 0 )
			return null;

		int rand = Random.Range(0, nextNodes.Count - 1);

		return nextNodes[rand];
	}

	void OnDrawGizmosSelected() 
	{
		if( nextNodes != null ) 
		{
			if( flyingNode )
				Gizmos.color = Color.green;
			else
				Gizmos.color = Color.red;
				
			if( nextNodes.Count > 0 ) 
			{
				foreach( MovementNode mn in nextNodes ) 
				{
					Gizmos.DrawLine( transform.position, mn.transform.position );
				}
			}
		}
	}
}
