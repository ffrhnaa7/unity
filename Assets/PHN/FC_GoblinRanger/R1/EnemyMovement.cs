using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public enum GoblinState //finite state machine concept
{
    Patrol, //when player is far
    Chase, //when player is near
    Attack, //when player is infront
    Dead //after HP becomes 0
}

public class GoblinAI : MonoBehaviour, IEnemy
{
    public NavMeshAgent navMeshAgent;
    public Animator m_Animator;
    public Transform[] waypoints;
    public LayerMask playerMask;
    public LayerMask obstacleMask;

    public float patrolSpeed = 1.4f;
    public float chaseSpeed = 3f;
    public float viewRadius = 15f;
    public float viewAngle = 90f;
    public float attackRange = 0.3f;
    public float maxHp = 100f;
    public float attackDamage = 10f;

    private float attackCooldown = 4f;
    private float nextAttackTime = 0f;
    private GoblinState currentState;
    private int currentWaypointIndex = 0;
    private Transform player;
    private float currentHp;
    private bool isDead = false;
    private GoblinWeaponHandler weaponHandler;
    private float attackPrepareDelay = 0.1f;
    private bool isPreparingAttack = false;

    [SerializeField] private ParticleSystem bloodEffect;

    private void Awake()
    {
       
        m_Animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        weaponHandler = GetComponent<GoblinWeaponHandler>();
        currentHp = maxHp;
    }

    private void Start() 
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null)
            Debug.LogError("❌ GoblinAI: Player not found! Tag your player ‘Player’.");
        else
            Debug.Log("✅ GoblinAI: Player found — " + player.name);

        ChangeState(GoblinState.Patrol);
    }

    private void Update()
    {
        if (isDead || player == null) return;
        if (m_Animator.applyRootMotion && navMeshAgent.enabled)
    navMeshAgent.velocity = Vector3.zero;

        switch (currentState)
        {
            case GoblinState.Patrol: Patrol(); break;
            case GoblinState.Chase: Chase(); break;
            case GoblinState.Attack: Attack(); break;
            case GoblinState.Dead: break;
        }
    }

    private void ChangeState(GoblinState newState)
    {
        Debug.Log($"⚙️ Goblin state changed to {newState}");
        currentState = newState;
    }

    private void Patrol()
    {
        m_Animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
        navMeshAgent.speed = patrolSpeed; //set speed under navMash
        navMeshAgent.acceleration = 4f;
        if (waypoints.Length == 0) return;

        if (!navMeshAgent.hasPath || navMeshAgent.remainingDistance < 0.5f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
        }

        if (CanSeePlayer()) ChangeState(GoblinState.Chase); //to chase
    }

   private void Chase()
    {
        navMeshAgent.isStopped = false;
        float speed = navMeshAgent.velocity.magnitude;
        m_Animator.SetFloat("Speed", speed);
        navMeshAgent.speed = chaseSpeed;
        navMeshAgent.SetDestination(player.position);

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= attackRange) ChangeState(GoblinState.Attack);
        else if (!CanSeePlayer()) ChangeState(GoblinState.Patrol);
    }


        private void Attack()
    {
        if (!isPreparingAttack)
        {
            navMeshAgent.isStopped = true;
            m_Animator.SetFloat("Speed", 0f);
            isPreparingAttack = true;
            StartCoroutine(PrepareAttack());
        }

        FacePlayer();

        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            isPreparingAttack = false;
            navMeshAgent.isStopped = false;
            ChangeState(GoblinState.Chase);
        }
    }

    IEnumerator PrepareAttack()
    {
        yield return new WaitForSeconds(attackPrepareDelay);

            float distance = Vector3.Distance(transform.position, player.position);

            if (currentState == GoblinState.Attack && distance <= attackRange)
            {
                m_Animator.SetTrigger("Attack");
                nextAttackTime = Time.time + attackCooldown;
            }

            isPreparingAttack = false;
    }


    public void DealDamage()
    {
        if (player != null && Vector3.Distance(transform.position, player.position) <= attackRange + 0.5f)
        {
            player.SendMessage("GetDamage", attackDamage, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void FacePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    private bool CanSeePlayer()
    {
        float d = Vector3.Distance(transform.position, player.position);
        if (d > viewRadius) return false;

        Vector3 dir = (player.position - transform.position).normalized;
        float a = Vector3.Angle(transform.forward, dir);
        if (a > viewAngle / 2) return false;

        Vector3 origin = transform.position + Vector3.up * 1f;
        if (Physics.Raycast(origin, dir, out RaycastHit hit, viewRadius, obstacleMask))
        {
            Debug.Log($"🚧 View blocked by {hit.collider.name}");
            return false;
        }

        Debug.Log("✅ Goblin sees the player!");
        return true;
    }

    public void OnAttackStart()
    {
        if (weaponHandler != null)
            weaponHandler.EnableWeaponCollider();
    }

    public void OnAttackEnd()
    {
        if (weaponHandler != null)
            weaponHandler.DisableWeaponCollider();
    }
    
        public void OnPlayerHit()
    {
        Debug.Log("GoblinAI: Player has been hit!");
        // You could also trigger a celebration animation, sound, etc.
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 leftRay = DirFromAngle(-viewAngle / 2);
        Vector3 rightRay = DirFromAngle(viewAngle / 2);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + leftRay * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + rightRay * viewRadius);
    }

    private Vector3 DirFromAngle(float angle)
    {
        angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    public void GetDamage(float damage)
{
    if (isDead) return;

    currentHp -= damage;
    Debug.Log($"🩸 Goblin took {damage} damage. Remaining HP: {currentHp}");
    
    if (bloodEffect != null)
        bloodEffect.Play();

    // 🔁 Knockback logic here
    Vector3 knockbackDir = (transform.position - player.position).normalized;
    navMeshAgent.Move(knockbackDir * 0.5f); // Push back slightly

    if (currentHp <= 0) Die();
}


    private void EnableRagdoll(bool active)
    {
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
            rb.isKinematic = !active;

        foreach (Collider col in GetComponentsInChildren<Collider>())
        {
            if (col.gameObject != this.gameObject)
                col.enabled = active;
        }

        GetComponent<Animator>().enabled = !active;
    }

    private void Die()
    {
        ChangeState(GoblinState.Dead);
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;

        m_Animator.enabled = false;
        EnableRagdoll(true);

        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 5f);
    }
}
