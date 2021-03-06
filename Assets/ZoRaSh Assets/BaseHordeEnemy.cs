﻿using UnityEngine;
using System.Collections;

public class BaseHordeEnemy : MonoBehaviour {
	
	public bool m_isMoving = false;
	public float m_deathWaitTime = 2f;
	public bool m_hasBeenHit = false;
	public int m_pointWorth = 1;

	protected Animator m_animator;
	protected Transform m_thisTransform;
	protected HordeWaypoint m_nextWaypoint;
	protected bool m_dead = false;
	protected float m_deathTimer = 0f;

	public virtual void StartMoving( HordeWaypoint startWp ) {
	}

	protected virtual void Reset() {

	}

	protected virtual void Hit( GameObject hittingObj ) {

	}
}
