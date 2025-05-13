using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    public Transform player;
    public Animator animator;
    public NavMeshAgent agent;
    public float bossHp = 100;
    private bool isAttacking = false;
    private bool isActive = false;

    // DamageToPlayer ����
    public GameObject attackDetectCube;
    private Collider attackCollider;
    public GameObject fireAttackDetectCube;
    private Collider fireAttackCollider;

    // Attackng ����
    public float attackRange = 4.3f;
    public float attackCooldown = 1.7335f;
    public float attackDamage = 10.0f;

    // Firing ����
    public GameObject firePrefab;
    public Transform fireSpawnPoint;
    public float firingCooldown = 5.8f;
    private GameObject fireInstance;

    // ���� ��Ʈ�ѷ� ����
    public SoundController soundController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // ���� ���� �ݶ��̴��� ó������ false�� ����
        if (attackDetectCube != null)
        {
            attackCollider = attackDetectCube.GetComponent<BoxCollider>();
            attackCollider.enabled = false;

            fireAttackCollider = fireAttackDetectCube.GetComponent<BoxCollider>();
            fireAttackCollider.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ���� �ִϸ��̼��� ���������� ���
        if (!isActive) return;

        // �÷��̾���� �Ÿ� ���
        float distance = Vector3.Distance(transform.position, player.position);

        // �������� �ƴϸ鼭 �Ÿ��� ���ݹ��� ���̸� �����ϰ� ���̸� ����
        if (!isAttacking)
        {
            if (distance <= attackRange)
                TryAttack();
            else
                ChasePlayer();
        }

        // ������ �״��� ��� Ȯ��
        BossDeath();
    }

    // Chasing �Լ�
    void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);

        // �÷��̾� ������ �ٶ󺸴� �ڵ� (�ڿ�������)
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f; // Y�� ȸ�� ����

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }

        // Chasing �ִϸ��̼� ���
        animator.SetBool("isChasing", true);
    }

    // Attacking �ڷ�ƾ �Լ�
    IEnumerator AttackPlayer_1()
    {
        isAttacking = true;
        agent.isStopped = true;

        // �÷��̾� ������ �ٶ󺸴� �ڵ� (�ڿ�������)
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f; // Y�� ȸ�� ����

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }

        // Attacking_1 �ִϸ��̼� ���
        animator.SetTrigger("Attacking_1");

        // Attacking_1 ���� ���
        soundController.AttackSound_1();

        // ���� ��Ÿ�Ӹ�ŭ ���
        yield return new WaitForSeconds(attackCooldown);

        // ���� ����
        isAttacking = false;
    }

    // Attacking �ڷ�ƾ �Լ�
    IEnumerator AttackPlayer_2()
    {
        isAttacking = true;
        agent.isStopped = true;

        // �÷��̾� ������ �ٶ󺸴� �ڵ� (�ڿ�������)
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f; // Y�� ȸ�� ����

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }

        // Attacking_2 �ִϸ��̼� ���
        animator.SetTrigger("Attacking_2");

        // Attacking_2 ���� ���
        soundController.AttackSound_2();

        // ���� ��Ÿ�Ӹ�ŭ ���
        yield return new WaitForSeconds(attackCooldown);

        // ���� ����
        isAttacking = false;
    }

    // Firing �ڷ�ƾ �Լ�
    IEnumerator FiringPlayer()
    {
        isAttacking = true;
        agent.isStopped = true;

        // �÷��̾� ������ �ٶ󺸴� �ڵ� (�ڿ�������)
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f; // Y�� ȸ�� ����

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }

        // �� ���� �ִϸ��̼� ���
        animator.SetTrigger("Firing");

        // �� ������ ����
        SpawnFire();

        // �� ���� ���� ��� (�ִϸ��̼ǰ� ���� ����ȭ�� ���� Offset ����)
        soundController.FireAttackSoundDelay(0.5f);

        // �� ���� ��Ÿ�Ӹ�ŭ ��� (�ִϸ��̼ǰ� ������ ����ȭ�� ���� Offset ����)
        yield return new WaitForSeconds(firingCooldown + 0.5f);

        // �ִϸ��̼��� ������ ���� ����
        soundController.audioSource.Stop();

        // ���� ����
        isAttacking = false;
    }

    // SpawnFire �Լ�
    public void SpawnFire()
    {
        // �� �������� ������ �� ��ġ���� ����
        fireInstance = Instantiate(firePrefab, fireSpawnPoint.position, fireSpawnPoint.rotation, fireSpawnPoint);

        // �ִϸ��̼��� ������ ������ �ı� (�ִϸ��̼ǰ� ������ ����ȭ�� ���� Offset ����)
        Destroy(fireInstance, firingCooldown + 0.5f);
    }

    // �� ���� ������ �������� ���
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

    // Death �Լ�
    void BossDeath()
    {
        if(bossHp <= 0)
        {
            // Death �ִϸ��̼� ���
            animator.SetTrigger("Death");
            agent.isStopped = true;
            isActive = false;

            // �� ������ �ı� �� 10�� �� ���� ������Ʈ �ı�
            Destroy(fireInstance);
            Destroy(gameObject, 10.0f);

            // �״� ���� ���
            soundController.DeathSound();
        }
    }

    // ���� ���� �ݶ��̴� Ȱ��ȭ
    public void EnableAttackCollider()
    {
        if (attackCollider != null)
        {
            attackCollider.enabled = true;
            Debug.Log("Enable AttackCollider!");
        }
    }

    // ���� ���� �ݶ��̴� ��Ȱ��ȭ
    public void DisableAttackCollider()
    {
        if (attackCollider != null)
        {
            attackCollider.enabled = false;
            Debug.Log("Disable AttackCollider!");
        }
    }
    
    // �� ���� ���� �ݶ��̴� Ȱ��ȭ
    public void EnableFireAttackCollider()
    {
        if (fireAttackCollider != null)
        {
            fireAttackCollider.enabled = true;
            Debug.Log("Enable fireAttackCollider!");
        }
    }

    // �� ���� ���� �ݶ��̴� ��Ȱ��ȭ
    public void DisableFireAttackCollider()
    {
        if (fireAttackCollider != null)
        {
            fireAttackCollider.enabled = false;
            Debug.Log("Disable fireAttackCollider!");
        }
    }

    // ���� ���� �ִϸ��̼��� ������ ��
    public void ActivateAI()
    {
        isActive = true;
    }
}
