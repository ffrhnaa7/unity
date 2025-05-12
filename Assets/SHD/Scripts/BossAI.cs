using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour, IEnemy
{
    public Transform player;
    public Animator animator;
    public NavMeshAgent agent;

    // PlayerDamaged 변수
    public GameObject attackDetectCube;
    private Collider attackCollider;

    public GameObject fireAttackDetectCube;
    private Collider fireAttackCollider;

    public float bossHp = 100;

    // Attackng 변수
    public float attackRange = 4.3f;
    public float attackCooldown = 1.7335f;
    public float attackDamage = 10.0f;

    // Firing 변수
    public GameObject firePrefab;
    public Transform fireSpawnPoint;
    public float firingCooldown = 5.8f;

    // Sound 변수
    public AudioSource audioSource;
    public AudioClip emergenceSound;
    public AudioClip attack_1Sound;
    public AudioClip attack_2Sound;
    public AudioClip fireAttackSound;
    public AudioClip footSoundLeft;
    public AudioClip footSoundRight;

    private bool isAttacking = false;
    private bool isActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 공격 감지 콜라이더를 처음에는 false로 설정
        if (attackDetectCube != null)
        {
            attackCollider = attackDetectCube.GetComponent<BoxCollider>();
            attackCollider.enabled = false;

            fireAttackCollider = fireAttackDetectCube.GetComponent<BoxCollider>();
            fireAttackCollider.enabled = false;
        }

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return; // 등장 애니메이션이 끝날때까지 대기

        float distance = Vector3.Distance(transform.position, player.position);

        if (!isAttacking)
        {
            if (distance <= attackRange)
                TryAttack();
            else
                ChasePlayer();
        }
    }

    // Attacking 함수
    IEnumerator AttackPlayer_1()
    {
        isAttacking = true;
        agent.isStopped = true;

        // 플레이어 방향을 바라보는 코드 (자연스럽게)
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f; // Y축 회전 방지

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }

        animator.SetTrigger("Attacking_1");

        audioSource.clip = attack_1Sound;
        audioSource.pitch = 1.2f;
        audioSource.time = 0f;
        audioSource.Play();
        //Invoke("StopSound", 1.2f);

        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }

    // Attacking 함수
    IEnumerator AttackPlayer_2()
    {
        isAttacking = true;
        agent.isStopped = true;

        // 플레이어 방향을 바라보는 코드 (자연스럽게)
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f; // Y축 회전 방지

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }

        animator.SetTrigger("Attacking_2");

        audioSource.clip = attack_2Sound;
        audioSource.pitch = 1.25f;
        audioSource.time = 0f;
        audioSource.Play();
        //Invoke("StopSound", 1.0f);
        
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }

    // Firing 함수
    IEnumerator FiringPlayer()
    {
        isAttacking = true;
        agent.isStopped = true;

        // 플레이어 방향을 바라보는 코드 (자연스럽게)
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f; // Y축 회전 방지

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }

        animator.SetTrigger("Firing");
        SpawnFire();

        Invoke("FireSound", 0.5f);

        yield return new WaitForSeconds(firingCooldown + 0.5f); // 애니메이션과 프리펩 동기화

        audioSource.Stop();

        isAttacking = false;
    }

    // SpawnFire 함수
    public void SpawnFire()
    {
        GameObject fireInstance = Instantiate(firePrefab, fireSpawnPoint.position, fireSpawnPoint.rotation, fireSpawnPoint);
        Destroy(fireInstance, firingCooldown + 0.5f); // 애니메이션과 프리펩 동기화
    }

    void TryAttack()
    {
        if (isAttacking) return;

        int randomAttack = UnityEngine.Random.Range(0, 3);

        if (randomAttack == 0)
            StartCoroutine(AttackPlayer_1());
        else if (randomAttack == 1)
            StartCoroutine(AttackPlayer_2());
        else
            StartCoroutine(FiringPlayer());
    }

    // Chasing 함수
    void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);

        // 플레이어 방향을 바라보는 코드 (자연스럽게)
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }

        animator.SetBool("isChasing", true);
    }

    private void FireSound()
    {
        audioSource.clip = fireAttackSound;
        audioSource.pitch = 1.0f;
        audioSource.time = 2.3f;
        audioSource.Play();
    }

    public void FootStepLeftSound()
    {
        audioSource.PlayOneShot(footSoundLeft);
    }

    public void FootStepRightSound()
    {
        audioSource.PlayOneShot(footSoundRight);
    }

    // 공격 감지 콜라이더 활성화
    public void EnableAttackCollider()
    {
        if (attackCollider != null)
        {
            attackCollider.enabled = true;
            Debug.Log("Enable AttackCollider!");
        }
    }

    // 공격 감지 콜라이더 비활성화
    public void DisableAttackCollider()
    {
        if (attackCollider != null)
        {
            attackCollider.enabled = false;
            Debug.Log("Disable AttackCollider!");
        }
    }
    
    // 불 공격 감지 콜라이더 활성화
    public void EnableFireAttackCollider()
    {
        if (fireAttackCollider != null)
        {
            fireAttackCollider.enabled = true;
            Debug.Log("Enable fireAttackCollider!");
        }
    }

    // 불 공격 감지 콜라이더 비활성화
    public void DisableFireAttackCollider()
    {
        if (fireAttackCollider != null)
        {
            fireAttackCollider.enabled = false;
            Debug.Log("Disable fireAttackCollider!");
        }
    }

    // Damaged 함수 (인터페이스)
    public void GetDamage(float damage)
    {
        bossHp -= damage;
    }

    // 보스 등장 애니메이션이 끝났을 때
    public void ActivateAI()
    {
        isActive = true;
    }
}
