using UnityEngine;
using System.Collections;

public class StandardZombie : MonoBehaviour {

	enum EnemyState {Spawn, Move, Attack, Dead}

	//Component References
	private Animator m_animator;
	private CharacterController m_cc;
	private Transform m_thisTransform;


	private EnemyState m_state;

    private Stats m_stats;

	void Start () {
//		if(m_animator == null)
		m_animator = GetComponent<Animator>();

		//if(m_thisTransform == null)
			m_thisTransform = GetComponent<Transform>();

		//if(m_cc == null)
			m_cc = GetComponent<CharacterController>();

		m_state = EnemyState.Spawn;

        m_stats = GetComponent<Stats>();
	}

	void Update () {
		if(Network.isServer) {
			switch(m_state)
			{
			case EnemyState.Spawn:
				m_state = EnemyState.Move;
				break;
			case EnemyState.Move:
				Move();
				break;
			case EnemyState.Attack:
				Attack();
				break;
			case EnemyState.Dead:
				break;
			}
		}
	}
	
	void TakeDamage(int damage) {
        m_stats.ApplyDamage(damage);

		if(m_stats.m_currHealth <= 0)
			m_state = EnemyState.Dead;
	}

	void Move () {
		if(Vector3.Distance(m_thisTransform.position, ManagerScript.instance.transform.position) <= m_stats.m_attackDistance) {
			m_state = EnemyState.Attack;
		}

		m_animator.SetFloat("Speed", m_thisTransform.forward.sqrMagnitude);

		m_cc.Move(m_thisTransform.forward * Time.deltaTime);
	}

	void Attack() {
        if (m_stats.m_attackTimer >= m_stats.m_attackCooldown)
        {
            //ManagerScript.instance.SendMessage("TakeDamage", m_stats.m_attackDamage);
			m_animator.SetBool("Attack", true);
            m_stats.m_attackTimer = 0;
		}
        else {
			m_animator.SetBool("Attack", false);
		}

        m_stats.m_attackTimer += Time.deltaTime;
	}

}
