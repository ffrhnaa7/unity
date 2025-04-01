using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    public Transform player;
    public Animator animator;
    public NavMeshAgent agent;

    public float attackRange = 2.0f;

    private bool isAttacking = false;
    private bool isActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return; // ���� �ִϸ��̼��� ���������� ���

        ChasePlayer();
    }

    // Chasing �Լ�
    void ChasePlayer()
    {
        if (isAttacking) // ���� ���̸� Chase���� ����
            return;

        agent.SetDestination(player.position);

        // �÷��̾� ������ �ٶ󺸴� �ڵ�
        Vector3 targetPos = player.position;
        targetPos.y = transform.position.y;
        agent.transform.LookAt(targetPos);

        animator.SetBool("isChasing", true);
    }

    // ���� ���� �ִϸ��̼��� ������ ��
    public void ActivateAI()
    {
        isActive = true;
    }
}
