using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    public Transform player;
    public Animator animator;
    public NavMeshAgent agent;
    public float bossMaxHp = 300;
    public float bossCurrentHp = 300;
    private bool isAttacking = false;
    private bool isActive = false;

    // DamageToPlayer 변수
    public GameObject attackDetectCube;
    private Collider attackCollider;
    public GameObject fireAttackDetectCube;
    private Collider fireAttackCollider;

    // Attackng 변수
    public float attackRange = 3.7f;
    public float attackCooldown = 1.7335f;
    public float attackDamage = 15.0f;

    // Firing 변수
    public GameObject firePrefab;
    public Transform fireSpawnPoint;
    public float firingCooldown = 5.4f;
    private GameObject fireInstance;

    // 사운드 컨트롤러 변수
    public SoundController soundController;

    // 보스 위치 컨트롤 오브젝트
    public GameObject StartLocation;
    public GameObject EndLocation;

    // DisappearScene 변수
    private bool hasDisappeared = false;
    private Material bossMaterial;
    private Renderer bossRenderer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 보스 위치를 시작 위치로 설정
        transform.position = StartLocation.transform.position;

        // 공격 감지 콜라이더를 처음에는 false로 설정
        if (attackDetectCube != null)
        {
            attackCollider = attackDetectCube.GetComponent<BoxCollider>();
            attackCollider.enabled = false;

            fireAttackCollider = fireAttackDetectCube.GetComponent<BoxCollider>();
            fireAttackCollider.enabled = false;
        }

        bossCurrentHp = bossMaxHp;

        // BossMaterial & BossRenderer 초기화
        bossRenderer = GetComponentInChildren<Renderer>();
        bossMaterial = bossRenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        // 등장 애니메이션이 끝날때까지 대기
        if (!isActive) return;

        // 플레이어와의 거리 계산
        float distance = Vector3.Distance(transform.position, player.position);

        // 공격중이 아니면서 거리가 공격범위 안이면 공격하고 밖이면 추적
        if (!isAttacking)
        {
            if (distance <= 1.5)
                StartCoroutine(AttackPlayer_2());
            else if (distance <= attackRange)
                TryAttack();
            else if (distance > attackRange)
                ChasePlayer();
        }

        // 보스가 사라지는지 계속 확인
        IsDisappeared();

        // 보스가 죽는지 계속 확인
        BossDeath();
    }

    // Chasing 함수
    void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);

        // 플레이어 방향을 바라보는 코드 (자연스럽게)
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f; // Y축 회전 방지

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }

        // Chasing 애니메이션 재생
        animator.SetBool("isChasing", true);
    }

    // Attacking 코루틴 함수
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

        // Attacking_1 애니메이션 재생
        animator.SetTrigger("Attacking_1");

        // Attacking_1 사운드 재생
        soundController.AttackSound_1();

        // 공격 쿨타임만큼 대기
        yield return new WaitForSeconds(attackCooldown);

        // 공격 해제
        isAttacking = false;
    }

    // Attacking 코루틴 함수
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

        // Attacking_2 애니메이션 재생
        animator.SetTrigger("Attacking_2");

        // Attacking_2 사운드 재생
        soundController.AttackSound_2();

        // 공격 쿨타임만큼 대기
        yield return new WaitForSeconds(attackCooldown);

        // 공격 해제
        isAttacking = false;
    }

    // Firing 코루틴 함수
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

        // 불 공격 애니메이션 재생
        animator.SetTrigger("Firing");

        // 불 프리팹 생성
        SpawnFire();

        // 불 공격 사운드 재생 (애니메이션과 사운드 동기화를 위한 Offset 설정)
        soundController.FireAttackSoundDelay(0.5f);

        // 불 공격 쿨타임만큼 대기 (애니메이션과 프리팹 동기화를 위한 Offset 설정)
        yield return new WaitForSeconds(firingCooldown + 0.5f);

        // 애니메이션이 끝나면 사운드 정지
        soundController.audioSource.Stop();

        // 공격 해제
        isAttacking = false;
    }

    // SpawnFire 함수
    public void SpawnFire()
    {
        // 불 프리팹을 보스의 입 위치에서 생성
        fireInstance = Instantiate(firePrefab, fireSpawnPoint.position, fireSpawnPoint.rotation, fireSpawnPoint);

        // 애니메이션이 끝나면 프리팹 파괴 (애니메이션과 프리팹 동기화를 위한 Offset 설정)
        Destroy(fireInstance, firingCooldown + 0.5f);
    }

    // 세 가지 공격을 랜덤으로 재생
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

    // DisappearScene 함수
    void IsDisappeared()
    {
        if (bossCurrentHp <= bossMaxHp / 2 && !hasDisappeared)
        {
            StartCoroutine(Disappeared());
            hasDisappeared = true;
            return;
        }
    }

    // DisappearScene 코루틴
    IEnumerator Disappeared()
    {
        isActive = false;
        Destroy(fireInstance);
        soundController.ScreamSound();
        ChangeRendererModeToFade();
        animator.SetTrigger("Disappear");

        yield return new WaitForSeconds(1.5f);

        float fadeDuration = 2.0f;
        float elapsedTime = 0.0f;

        Color startColor = bossMaterial.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (elapsedTime <= fadeDuration)
        {
            bossMaterial.color = Color.Lerp(startColor, endColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        bossMaterial.color = endColor;
        gameObject.SetActive(false);

        transform.position = EndLocation.transform.position;
        transform.rotation = Quaternion.Euler(0f, -90f, 0f);
    }

    // Boss Renderer Mode = fade
    void ChangeRendererModeToFade()
    {
        bossMaterial.SetFloat("_Mode", 2); // 2 = Fade
        bossMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        bossMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        bossMaterial.SetInt("_ZWrite", 0);
        bossMaterial.DisableKeyword("_ALPHATEST_ON");
        bossMaterial.EnableKeyword("_ALPHABLEND_ON");
        bossMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        bossMaterial.renderQueue = 3000;
    }

    // Boss Renderer Mode = opaque
    public void ChangeRendererModeToOpaque()
    {
        bossMaterial.SetFloat("_Mode", 0); // 0 = Opaque
        bossMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        bossMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        bossMaterial.SetInt("_ZWrite", 1);
        bossMaterial.EnableKeyword("_ALPHATEST_ON");
        bossMaterial.DisableKeyword("_ALPHABLEND_ON");
        bossMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        bossMaterial.renderQueue = -1;

        // 알파 값도 다시 1로 설정 (완전 불투명)
        Color color = bossMaterial.color;
        color.a = 1f;
        bossMaterial.color = color;
    }

    // Death 함수
    void BossDeath()
    {
        if(bossCurrentHp <= 0)
        {
            // Death 애니메이션 재생
            animator.SetTrigger("Death");
            agent.isStopped = true;
            isActive = false;

            // 불 프리팹 파괴 및 10초 후 보스 오브젝트 파괴
            Destroy(fireInstance);
            Destroy(gameObject, 10.0f);

            // 죽는 사운드 재생
            soundController.DeathSound();
        }
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

    // 보스 등장 애니메이션이 끝났을 때
    public void ActivateAI()
    {
        isActive = true;

        // 시작 버그 수정 코드
        ChasePlayer();
    }

    public void ActivateAIForPage2()
    {
        isActive = true;
        //animator.SetBool("isChasing", false);
    }
}
