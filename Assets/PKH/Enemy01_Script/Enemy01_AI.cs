using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class Enemy01_AI : MonoBehaviour
{
    public enum State { Idle, Patrol, Chase, Attack, Return, Dead }
    public State currentState = State.Idle;

    [Header("Settings")]
    public float sightRange = 10f;
    public float attackRange = 2f;
    public float returnRange = 15f;
    public float walkSpeed = 1f;
    public float runSpeed = 2f;

    private Transform player;
    private NavMeshAgent agent;
    private Animator anim;
    private Vector3 originPosition;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        originPosition = transform.position;

        currentState = State.Idle;
    }

    void Update()
    {
        if (currentState == State.Dead) return;

        float distance = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Idle:
                Idle();
                if (distance < sightRange) currentState = State.Chase;
                break;

            case State.Patrol:
                Patrol();
                if (distance < sightRange) currentState = State.Chase;
                break;

            case State.Chase:
                Chase();
                if (distance < attackRange) currentState = State.Attack;
                else if (distance > returnRange) currentState = State.Return;
                break;

            case State.Attack:
                Attack();
                if (distance > attackRange) currentState = State.Chase;
                break;

            case State.Return:
                Return();
                if (Vector3.Distance(transform.position, originPosition) < 1f) currentState = State.Patrol;
                break;
        }
    }

    void Idle()
    {
        anim.SetBool("isWalking", false);
        anim.SetBool("isRunning", false);
        agent.SetDestination(transform.position);
    }

    void Patrol()
    {
        agent.speed = walkSpeed;
        anim.SetBool("isWalking", true);
        anim.SetBool("isRunning", false);
        agent.SetDestination(originPosition); 
    }

    void Chase()
    {
        agent.speed = runSpeed;
        anim.SetBool("isWalking", false);
        anim.SetBool("isRunning", true); // ← 여기에 추가
        agent.SetDestination(player.position);
    }

    void Attack()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);
        anim.SetTrigger("attack");
    }

    void Return()
    {
        agent.speed = walkSpeed;
        anim.SetBool("isWalking", true);
        anim.SetBool("isRunning", false);        
        agent.SetDestination(originPosition);
    }

    public void TakeDamage(float damage)
    {
        currentState = State.Dead;
        Die();
    }

    void Die()
    {
        anim.SetBool("isDead", true);
        agent.enabled = false;
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 5f);
    }
}
