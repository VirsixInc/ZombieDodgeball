using UnityEngine;
using System.Collections;

public class BaseHordeEnemy : MonoBehaviour {
	
	public bool m_isMoving = false;
	public float m_deathWaitTime = 2f;

	protected Animator m_animator;
	protected Transform m_thisTransform;
	protected HordeWaypoint m_nextWaypoint;
	protected bool m_dead = false;
	protected bool m_hasBeenHit = false;
	protected float m_deathTimer = 0f;

	public virtual void StartMoving( HordeWaypoint startWp ) {
	}

	protected virtual void Reset() {

	}

	protected virtual void Hit( GameObject hittingObj ) {

	}
}
