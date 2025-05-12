using UnityEngine;
using UnityEngine.AI;

public enum GoblinState
{
    Patrol,
    Chase,
    Attack,
    Dead
}

public class GoblinAI : MonoBehaviour, IEnemy
{
    public NavMeshAgent navMeshAgent;
    public Animator m_Animator;
    public Transform[] waypoints;
    public LayerMask playerMask;
    public LayerMask obstacleMask;

    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float viewRadius = 15f;
    public float viewAngle = 90f;
    public float attackRange = 2.5f;
    public float maxHp = 100f;
    public float attackDamage = 10f;

    private float attackCooldown = 1.5f;
    private float nextAttackTime = 0f;
    private GoblinState currentState;
    private int currentWaypointIndex = 0;
    private Transform player;
    private float currentHp;
    private bool isDead = false;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentHp = maxHp;

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
            Debug.LogError("❌ GoblinAI: Player not found! Make sure the Player GameObject has the tag 'Player'");
        else
            Debug.Log("✅ GoblinAI: Player found — " + player.name);

        ChangeState(GoblinState.Patrol);
    }




    private void Update()
    {
        if (isDead || player == null) return;

        switch (currentState)
        {
           
            case GoblinState.Patrol:
                Patrol(); break;
            case GoblinState.Chase:
                Chase(); break;
            case GoblinState.Attack:
                Attack(); break;
            case GoblinState.Dead:
                break;
        }
    }

    private void ChangeState(GoblinState newState)
    {
        currentState = newState;
    }

    

    private void Patrol()
    {
        Debug.Log("Goblin is in Patrol state");

        m_Animator.SetFloat("Speed", patrolSpeed);
        navMeshAgent.speed = patrolSpeed;

        if (waypoints.Length == 0) return;

        if (!navMeshAgent.hasPath || navMeshAgent.remainingDistance < 0.5f)
        {
            Debug.Log("Goblin moving to waypoint: " + waypoints[currentWaypointIndex].name);
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
        }

        if (CanSeePlayer()) ChangeState(GoblinState.Chase);
    }


    private void Chase()
    {
        m_Animator.SetFloat("Speed", chaseSpeed);
        navMeshAgent.speed = chaseSpeed;
        navMeshAgent.SetDestination(player.position);

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= attackRange) ChangeState(GoblinState.Attack);
        else if (!CanSeePlayer()) ChangeState(GoblinState.Patrol);
    }

    private void Attack()
    {
        navMeshAgent.isStopped = true;

        if (Time.time >= nextAttackTime)
        {
            m_Animator.SetTrigger("Attack");
            nextAttackTime = Time.time + attackCooldown;
        }

        FacePlayer();

        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            navMeshAgent.isStopped = false;
            ChangeState(GoblinState.Chase);
        }
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
        float dist = Vector3.Distance(transform.position, player.position);
        Debug.Log("Checking distance to player: " + dist);

        if (dist > viewRadius) return false;

        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, dirToPlayer);
        Debug.Log("Angle to player: " + angle);

        if (angle < viewAngle / 2f)
        {
            if (!Physics.Raycast(transform.position, dirToPlayer, viewRadius, obstacleMask))
            {
                Debug.Log("✅ Goblin sees the player!");
                return true;
            }
            else
            {
                Debug.Log("❌ Goblin's view is blocked by something.");
            }
        }
        else
        {
            Debug.Log("❌ Player is outside of field of view.");
        }

        return false;
    }



    public void GetDamage(float damage)
    {
        if (isDead) return;
        currentHp -= damage;
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

        m_Animator.enabled = false; // 🔁 disable animation control
        EnableRagdoll(true);        // ✅ turn on ragdoll

        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 5f);
    }

}
