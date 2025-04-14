using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;


public enum GoblinState
{
    Idle,
    Patrol,
    Chase,
    Attack,
    Dead
}

public class GoblinAI : MonoBehaviour, IEnemy
{
    public NavMeshAgent navMeshAgent;
    public Animator m_Animator;
    public Transform[] waypoints;
    public LayerMask playerMask;
    public LayerMask obstacleMask;

    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float viewRadius = 15f;
    public float viewAngle = 90f;
    public float attackRange = 2.5f;
    public float maxHp = 100f;

    private GoblinState currentState;
    private int currentWaypointIndex = 0;
    private Transform player;
    private float currentHp;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentHp = maxHp;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        ChangeState(GoblinState.Idle);
    }

    private void Update()
    {
        switch (currentState)
        {
            case GoblinState.Idle:
                Idle();
                break;
            case GoblinState.Patrol:
                Patrol();
                break;
            case GoblinState.Chase:
                Chase();
                break;
            case GoblinState.Attack:
                Attack();
                break;
            case GoblinState.Dead:
                // Don't do anything if dead
                break;
        }
    }

    private void ChangeState(GoblinState newState)
    {
        currentState = newState;
    }

    private void Idle()
    {
        m_Animator.SetFloat("Speed", 0);

        if (CanSeePlayer())
        {
            ChangeState(GoblinState.Chase);
        }
        else
        {
            ChangeState(GoblinState.Patrol);
        }
    }

    private void Patrol()
    {
        m_Animator.SetFloat("Speed", patrolSpeed);
        navMeshAgent.speed = patrolSpeed;

        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
        }

        if (CanSeePlayer())
        {
            ChangeState(GoblinState.Chase);
        }
    }

    private void Chase()
    {
        m_Animator.SetFloat("Speed", chaseSpeed);
        navMeshAgent.speed = chaseSpeed;
        navMeshAgent.SetDestination(player.position);

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= attackRange)
        {
            ChangeState(GoblinState.Attack);
        }
        else if (!CanSeePlayer())
        {
            ChangeState(GoblinState.Patrol);
        }
    }

    private void Attack()
    {
        navMeshAgent.isStopped = true;
        m_Animator.SetBool("IsAttacking", true);

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > attackRange)
        {
            m_Animator.SetBool("IsAttacking", false);
            navMeshAgent.isStopped = false;
            ChangeState(GoblinState.Chase);
        }
    }

    private bool CanSeePlayer()
    {
        if (Vector3.Distance(transform.position, player.position) > viewRadius) return false;

        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
        {
            if (!Physics.Raycast(transform.position, dirToPlayer, viewRadius, obstacleMask))
            {
                return true;
            }
        }
        return false;
    }

    public void GetDamage(float damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        ChangeState(GoblinState.Dead);
        navMeshAgent.isStopped = true;
        m_Animator.SetTrigger("Die");
        Destroy(gameObject, 2f);
    }
}
