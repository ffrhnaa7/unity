using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using StarterAssets;

// 1. Enemy 의 상태를 처리할 구조를 작성
// 대기, 이동, 달리기, 공격
public class Enemy01_AI : MonoBehaviour, IEnemy
{
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Damage,
        Die
    };

    private EnemyState m_state;

    private CharacterController cc;

    private Animator anim;

    private NavMeshAgent agent;

    void Start()
    {
        m_state = EnemyState.Idle;

        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        print("현재 상태 : " + m_state);
        switch (m_state)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Damage:
                Damage();
                break;
            case EnemyState.Die:
                Die();
                break;
        }
    }
    
    // 필요 속성: 대기 시간, 경과 시간
    public float idleDelayTime = 2;
    private float currentTime = 0;
    private void Idle()
    {
        // 일정 시간이 지나면 Idle → Move로 전환
        // 1. 시간이 흘렀으니
        currentTime += Time.deltaTime;
        // 2. 일정 시간이 됐으니까
        if (currentTime > idleDelayTime)
        {
            // 3. 상태를 Move 로 전환
            m_state = EnemyState.Move;
            // 애니메이션 상태도 Move로 전환
            anim.SetTrigger("Move");
            currentTime = 0;
        }
        
    }
    
    // 필요속성 : 이동속도, 타겟
    public float speed = 5;
    public Transform target;
    
    // 필요 속성: 공격 범위
    public float attackRange = 2;
    private void Move()
    {
        // 타겟 방향으로 이동하고 싶다.
        // 1. 방향이 필요
        Vector3 dir = target.position - transform.position;
        float distance = dir.magnitude; // 거리를 구함
    
        // 공격 범위 안에 타겟이 들어오면 상태를 Attack 으로 전환
        if (distance < attackRange)
        {
            m_state = EnemyState.Attack;
            currentTime = attackDelayTime;
            return;
        }
        
        // NavMeshAgnet 설정 간 사용 안할 코드들
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
    
    
    // 타겟이 공격 범위를 벗어나면 상태를 Move로 상태 전환
    
    // 필요속성: 공격 대기 시간
    public float attackDelayTime = 2;
    private void Attack()
    {
        // 일정 시간에 한 번씩 공격하고 싶다.
        currentTime += Time.deltaTime;
        
        if (currentTime > attackDelayTime)
        {
            currentTime = 0;
            anim.SetTrigger("attack1");
            print("공격!!!!!"); // MonoBehavior 덕분에 사용가능(로그 찍기)
            
            // 타겟(플레이어)에게 데미지를 주기
            // target은 Transform이라 PlayerController가 있는지 검사
            if (target.TryGetComponent(out PlayerController player))
            {
                // 플레이어에게 데미지 전달
                player.GetDamage(1f); // 1은 고정된 적 공격력 (원하면 변수화 가능)
            }
        }
        
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > attackRange)
        {
            m_state = EnemyState.Move;
            anim.SetTrigger("Move");
        }
    }
    
    
    // 일정시간 지나면 상태를 Idle로 전환
    public float damageDelayTime = 2;

    private void Damage()
    {
        currentTime += Time.deltaTime;
        if (currentTime > damageDelayTime)
        {
            currentTime = 0;
            m_state = EnemyState.Idle;
        }
    }
    
    // GetDamae 코드

    private int hp = 15; // 초기 체력 설정
    
    public void GetDamage(float damage)
    {
        // 이미 상태가 Die이면 호출되지 않도록 하자
        if (m_state == EnemyState.Die)
        {
            return;
        }
        
        // 받은 데미지만큼 체력 감소
        hp -= (int)damage;
        currentTime = 0; // Idle 상태 타이머 리셋
        
        // 현재 체력 상태를 출력(디버깅용)
        Debug.Log($"{gameObject.name} damaged! Current HP: {hp}");
        
        // 체력이 0 이하면 죽음 처리
        if (hp <= 0)
        {
            m_state = EnemyState.Die;
            anim.SetTrigger("Die");
            // 충돌체 정지 기능
            cc.enabled = false;
        }
        else
        {
            // 죽지는 않았지만 피격 상태이므로 일시적으로 Idle 상태로 전환
            // 이는 "맞았을 때 행동을 멈추는" 연출로도 사용 가능
            m_state = EnemyState.Damage;
            anim.SetTrigger("Damage");
        }

    }
    
    // 아래로 계속 내려가다가 안보이면 제거시켜주자
    // 필요속성 : 죽을 때 속도, 사라질 위치
    public float dieSpeed = 0.5f;
    public float dieYPosition = -2;
    private void Die()
    { 
        // 일정시간 기다렸다가
        currentTime += Time.deltaTime;
        if (currentTime > 2)
        {
            // 아래로 가라앉도록 하자
            // P = P0 + vt
            transform.position += Vector3.down * dieSpeed * Time.deltaTime;
            if (transform.position.y < dieYPosition)
            {
                Destroy(gameObject);
            }

        }
        

        //Debug.Log($"{gameObject.name} has died.");
        //Destroy(gameObject, 10f); // 적을 2초 후 삭제
    }
    

}