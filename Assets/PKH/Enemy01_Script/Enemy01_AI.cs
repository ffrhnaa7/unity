using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. Enemy 의 상태를 처리할 구조를 작성
// 대기, 이동, 달리기, 공격
public class Enemy01_AI : MonoBehaviour, IEnemy
{
    enum EnemyState
    {
        Idle,
        Walk,
        Run,
        Attack
    };

    private EnemyState m_state;

    private CharacterController cc;

    void Start()
    {
        m_state = EnemyState.Idle;

        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        print("현재 상태 : " + m_state);
        switch (m_state)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Walk:
                Walk();
                break;
            case EnemyState.Run :
                Run();
                break;
            case EnemyState.Attack:
                Attack();
                break;
        }
    }
    
    // 필요 속성: 대기 시간, 경과 시간
    public float idleDelayTime = 2;
    private float currentTime = 0;
    private void Idle()
    {
        // 일정 시간이 지나면 Idle → Walk로 전환
        // 1. 시간이 흘렀으니
        currentTime += Time.deltaTime;
        // 2. 일정 시간이 됐으니까
        if (currentTime > idleDelayTime)
        {
            // 3. 상태를 Walk 로 전환
            m_state = EnemyState.Walk;
            currentTime = 0;
        }
        
    }
    
    // 필요속성 : 이동속도, 타겟
    public float speed = 5;
    public Transform target;
    
    // 필요 속성: 공격 범위
    public float attackRange = 2;
    private void Walk()
    {
        // 타겟 방향으로 이동하고 싶다.
        // 1. 방향이 필요
        Vector3 dir = target.position - transform.position;
        float distance = dir.magnitude; // 거리를 구함
    
        // 공격 범위 안에 타겟이 들어오면 상태를 Attack 으로 전환
        if (distance < attackRange)
        {
            m_state = EnemyState.Attack;
            return;
        }
        dir.y = 0; // 너무 크면 쳐다볼 때, 하늘을 바라보는 오류 수정 코드
        dir.Normalize();
        // 2. 이동하고 싶다.
        // P = P0 + vt
        cc.SimpleMove(dir * speed);
        
        // 이동하는 방향으로 회전하고 싶다.
        //transform.LookAt(target);
        //transform.forward = dir; // 부드럽게 회전은 안된다.
        // 부드럽게 회전하는 코드
        //transform.forward = Vector3.Lerp(transform.forward, dir, 5 * Time.deltaTime); -> 회전 오류 발생
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 10*Time.deltaTime);
    }

    // Visual Debugging 을 위한 함수
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void Run()
    {
        throw new System.NotImplementedException();
    }
    
    // 타겟이 공격 범위를 벗어나면 상태를 Walk로 상태 전환
    
    // 필요속성: 공격 대기 시간
    public float attackDelayTime = 2;
    private void Attack()
    {
        // 일정 시간에 한 번씩 공격하고 싶다.
        currentTime += Time.deltaTime;
        if (currentTime > attackDelayTime)
        {
            currentTime = 0;
            print("공격!!!!!"); // MonoBehavior 덕분에 사용가능(로그 찍기)
        }
        
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > attackRange)
        {
            m_state = EnemyState.Walk;
        }
    }
    public void GetDamage(float damage)
    {

    }

    private void Die()
    {
        Destroy(gameObject, 2f); // goblin disapeears after 2 sec
    }

    // 피격 당했을 때 호출되는 함수
    // hp 갖도록 하고싶다.
    private int hp = 3;
    // 만약 hp 가 0 이하면 죽이고
    // 그렇지 않으면 상태를 Idle 로 전환하기
    public void OnDamageProcess()
    {
        hp--;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            m_state = EnemyState.GetDamage;
            currentTime = 0;
        }
        
    }

}