using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour, IEnemy
{
    public Transform player;
    public Animator animator;
    public NavMeshAgent agent;

    // PlayerDamaged ����
    public GameObject attackDetectCube;
    private Collider attackCollider;

    public float bossHp = 100;

    // Attackng ����
    public float attackRange = 4.3f;
    public float attackCooldown = 1.7335f;
    public float attackDamage = 10.0f;

    private bool isAttacking = false;
    private bool isActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // ���� ���� �ݶ��̴��� ó������ false�� ����
        if (attackDetectCube != null)
        {
            attackCollider = attackDetectCube.GetComponent<Collider>();
            attackCollider.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return; // ���� �ִϸ��̼��� ���������� ���

        float distance = Vector3.Distance(transform.position, player.position);

        if (!isAttacking)
        {
            if (distance <= attackRange)
                TryAttack();
            else
                ChasePlayer();
        }
    }

    // Attacking �Լ�
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
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }

        animator.SetTrigger("Attacking_1");
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }

    // Attacking �Լ�
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
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }

        animator.SetTrigger("Attacking_2");
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }

    void TryAttack()
    {
        if (isAttacking) return;

        int randomAttack = UnityEngine.Random.Range(0, 2);

        if (randomAttack == 0)
            StartCoroutine(AttackPlayer_1());
        else
            StartCoroutine(AttackPlayer_2());
    }

    // Chasing �Լ�
    void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);

        // �÷��̾� ������ �ٶ󺸴� �ڵ� (�ڿ�������)
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }

        animator.SetBool("isChasing", true);
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

    // Damaged �Լ� (�������̽�)
    public void GetDamage(float damage)
    {
        bossHp -= damage;
    }

    // ���� ���� �ִϸ��̼��� ������ ��
    public void ActivateAI()
    {
        isActive = true;
    }
}
