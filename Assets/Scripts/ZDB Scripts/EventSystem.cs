using UnityEngine;
using System.Collections;

public class EventSystem : MonoBehaviour 
{
	protected Ball eventBall;

	protected virtual void Start () 
	{
	
	}
	
	protected virtual void Update () 
	{
	
	}
	
	public virtual void StartEvent( Ball eBall )
	{
		eventBall = eBall;
	}
}
