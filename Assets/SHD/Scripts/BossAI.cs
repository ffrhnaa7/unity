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
        if (!isActive) return; // 등장 애니메이션이 끝날때까지 대기

        ChasePlayer();
    }

    // Chasing 함수
    void ChasePlayer()
    {
        if (isAttacking) // 공격 중이면 Chase하지 않음
            return;

        agent.SetDestination(player.position);

        // 플레이어 방향을 바라보는 코드
        Vector3 targetPos = player.position;
        targetPos.y = transform.position.y;
        agent.transform.LookAt(targetPos);

        animator.SetBool("isChasing", true);
    }

    // 보스 등장 애니메이션이 끝났을 때
    public void ActivateAI()
    {
        isActive = true;
    }
}
