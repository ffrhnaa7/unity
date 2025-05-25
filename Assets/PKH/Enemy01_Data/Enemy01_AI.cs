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
        Die,
        Return
    };

    private EnemyState m_state;

    private CharacterController cc;

    private Animator anim;

    private NavMeshAgent agent;

    private Vector3 originPosition;
    
    void Start()
    {
        m_state = EnemyState.Idle;

        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        
        originPosition = transform.position; // 💡 시작 위치 저장
    }

    void Update()
    {
        
        float playerDistance = Vector3.Distance(transform.position, target.position);

        if (isPlayerDetected && playerDistance > detectRange + 1f)
        {
            // 플레이어가 사라짐 → 복귀 시작
            isPlayerDetected = false;
            m_state = EnemyState.Return;
            anim.SetTrigger("Move"); // 이동 애니메이션
        }
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
            case EnemyState.Return:
                Return();
                break;
        }
    }

    public void Return()
    {
        Vector3 dir = originPosition - transform.position;
        float distance = dir.magnitude;

        if (distance < 0.5f)
        {
            // 원위치 도달 → Idle 상태로 전환
            m_state = EnemyState.Idle;

            // 트리거 리셋 후 Idle 트리거 설정
            anim.ResetTrigger("Move");   // 🔁 혹시 Move가 남아 있으면 방지
            anim.SetTrigger("Idle");     // ✅ Idle 애니메이션 실행

            return;
        }

        dir.y = 0;
        dir.Normalize();
        cc.SimpleMove(dir * speed);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
    }
    

    // 필요 속성: 대기 시간, 경과 시간
    public float idleDelayTime = 2;
    private float currentTime = 0;
    private void Idle()
    {
        // 일정 시간이 지나면 Idle → Move로 전환
        // 1. 시간이 흘렀으니
        currentTime += Time.deltaTime;
        
        // 플레이어와의 거리 계산(탐지 후 쫓기 위한 코드)
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);
        
        // 2. 일정 시간이 됐으니까
        // currentTime > idleDelayTime(탐지 전)
        if (distanceToPlayer < detectRange)
        {
            isPlayerDetected = true; // 플레이어와의 거리 계산(탐지 후 쫓기 위한 코드)
            // 3. 상태를 Move 로 전환
            m_state = EnemyState.Move;
            // 애니메이션 상태도 Move로 전환
            anim.SetTrigger("Move");
            currentTime = 0;
            return; // 플레이어와의 거리 계산(탐지 후 쫓기 위한 코드) 
        }
        
        // 플레이어와의 거리 계산(탐지 후 쫓기 위한 코드)
        // 시간 기준 이동은 제거하거나 보조용으로 유지 가능
        if (currentTime > idleDelayTime)
        {
            currentTime = 0;
            // isPlayerDetected가 false면 여전히 Idle 유지
        }
        
    }
    
    // 필요속성 : 이동속도, 타겟
    public float speed = 5;
    public Transform target;
    
    // 필요 속성: 공격 범위
    public float attackRange = 1;
    private void Move()
    {
        // 플레이어와의 거리 계산(탐지 후 쫓기 위한 코드)
        if (!isPlayerDetected)
            return;
        
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
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    
    // 플레이어를 탐지할 거리
    public float detectRange = 10f; // 플레이어를 탐지할 거리
    private bool isPlayerDetected = false;
    
    // Attack01 Animation 관련 Weapon Collider 사용으로 공격 타이밍 맞추기 코드
    public Enemy01Weapon weapon;
    public void EnableWeaponTrue()
    {
        if (weapon != null)
        {
            weapon.EnableWeapon(true);
            Debug.Log("Enable AttackCollider!");
        }
            
    }

    public void EnableWeaponFalse()
    {
        if (weapon != null)
        {
            weapon.EnableWeapon(false);
            Debug.Log("Disable AttackCollider!");
        }
    }

    
    
    
    // 타겟이 공격 범위를 벗어나면 상태를 Move로 상태 전환

    private bool isAttacking = false;
    
    // 필요속성: 공격 대기 시간
    public float attackDelayTime = 2;
    private void Attack()
    {
        // 공격 중이 아니면 공격 시작
        if (!isAttacking)
        {
            isAttacking = true;
            currentTime = 0;
            anim.SetTrigger("attack1");
            Debug.Log("공격!!!!!");
        }

        // 공격 도중 대기 시간 측정
        currentTime += Time.deltaTime;

        if (currentTime > attackDelayTime)
        {
            isAttacking = false;

            // 공격 종료 후 거리 검사
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance > attackRange)
            {
                m_state = EnemyState.Move;
                anim.SetTrigger("Move");
            }
            else
            {
                // 다시 공격 반복 (혹은 Idle로 전환 가능)
                m_state = EnemyState.Attack;
            }

            currentTime = 0;
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
            currentTime = 0;
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