using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using StarterAssets;

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
    public float chaseSpeed = 2.6f;
    public float viewRadius = 15f;
    public float viewAngle = 100f;
    public float attackRange = 2.5f;
    public float maxHp = 30f;
    public float attackDamage = 10f;

    private float attackCooldown = 0.1f;
    private float nextAttackTime = 0f;
    private GoblinState currentState;
    private int currentWaypointIndex = 0;
    private Transform player;
    private float currentHp;
    private bool isDead = false;
    private GoblinWeaponHandler weaponHandler;
    private float attackPrepareDelay = 0.1f;
    private bool isPreparingAttack = false;
    private AudioSource audioSource;
    private float nextPatrolSoundTime = 0f;

    [SerializeField] private ParticleSystem bloodEffect;
    [SerializeField] private AudioClip patrolSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private GameObject surpriseIcon;
    [SerializeField] private float iconDisplayDuration = 1.5f;
    [SerializeField] private float patrolSoundInterval = 5f;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        weaponHandler = GetComponent<GoblinWeaponHandler>();
        currentHp = maxHp;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        //audioSource.volume = 0.3f;
            
        if (surpriseIcon != null)
            surpriseIcon.SetActive(false);
    }

    private void Start() //as soon game begins
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
        {
            navMeshAgent.ResetPath(); // prevent further movement
            navMeshAgent.velocity = Vector3.zero;
        }

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

//--------------patrol section---------------------------------------------------------------
    private void Patrol() //patrol
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

        if (Time.time >= nextPatrolSoundTime && patrolSound != null)
        {
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(patrolSound);
            nextPatrolSoundTime = Time.time + patrolSoundInterval;
        }

        if (CanSeePlayer()) ChangeState(GoblinState.Chase); //to chase
    }
//--------------chase section---------------------------------------------------------------
    private void Chase() //to chase
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = chaseSpeed;
        navMeshAgent.SetDestination(player.position);

        float speed = navMeshAgent.velocity.magnitude;
        m_Animator.SetFloat("Speed", Mathf.Clamp(speed, 0f, chaseSpeed));

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= attackRange) ChangeState(GoblinState.Attack);
        else if (!CanSeePlayer()) ChangeState(GoblinState.Patrol);
    }

//--------------attack section---------------------------------------------------------------
    private void Attack()  //attack
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

            m_Animator.ResetTrigger("Attack"); 
            ChangeState(GoblinState.Chase);
        }
    }

    IEnumerator PrepareAttack() //gap time before attacl
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


    public void DealDamage() //to player's side
    {
        if (player != null && Vector3.Distance(transform.position, player.position) <= attackRange + 0.5f)
        {
            player.SendMessage("GetDamage", attackDamage, SendMessageOptions.DontRequireReceiver);
        }
    }
//--------------direction when saw player section---------------------------------------------------------------
    private void FacePlayer() //ensure it faces player when attack
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }
//--------------surprised effect---------------------------------------------------------------
    public void ShowSurpriseIcon()
    {
        if (surpriseIcon != null)
            StartCoroutine(FlashSurpriseIcon());
    }

    private IEnumerator FlashSurpriseIcon()
    {
        surpriseIcon.SetActive(true);
        yield return new WaitForSeconds(iconDisplayDuration);
        surpriseIcon.SetActive(false);
    }
//--------------find player section---------------------------------------------------------------
    private bool CanSeePlayer()
    {
        float d = Vector3.Distance(transform.position, player.position);
        if (d > viewRadius) return false; //what was this.

        Vector3 dir = (player.position - transform.position).normalized;
        float a = Vector3.Angle(transform.forward, dir);
        if (a > viewAngle / 2) return false; //this?

        Vector3 origin = transform.position + Vector3.up * 1f;
        if (Physics.Raycast(origin, dir, out RaycastHit hit, viewRadius, obstacleMask))
        {
            Debug.Log($"🚧 View blocked by {hit.collider.name}"); //debug on why or what decides 'being blocked'
            return false;
        }

        Debug.Log("✅ Goblin sees the player!");
        return true;
    }
//--------------weapon section---------------------------------------------------------------
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

    public void OnPlayerHit() //after attack 
    {
        Debug.Log("GoblinAI: Player has been hit!");
             if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound);
        } 
       
    }
//--------------death section---------------------------------------------------------------

    private void OnDrawGizmosSelected() //for checking 
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 leftRay = DirFromAngle(-viewAngle / 2);
        Vector3 rightRay = DirFromAngle(viewAngle / 2);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + leftRay * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + rightRay * viewRadius);
    }

    private Vector3 DirFromAngle(float angle) // fov in 3d space
    {
        angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    public void GetDamage(float damage) //for HP health
    {
        if (isDead) return;

        currentHp -= damage;
        Debug.Log($"🩸 Goblin took {damage} damage. Remaining HP: {currentHp}");

        if (bloodEffect != null)
            bloodEffect.Play();

        Vector3 knockbackDir = (transform.position - player.position).normalized;
        navMeshAgent.Move(knockbackDir * 0.5f); // push back

        if (currentState == GoblinState.Patrol)
        {
            ShowSurpriseIcon();
            Debug.Log("⚠️ Goblin: Surprised by attack!");
            ChangeState(GoblinState.Chase);
        }

        if (currentHp <= 0) Die();
    }


    private void EnableRagdoll(bool active) //die
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

    private void Die() //die 
    {
        //even after dying how can i make it reappear again but randomly (within the nav space (not using waypoints))
        //also how to add UI 'you have defeated to goblin,move ahead!'
        ChangeState(GoblinState.Dead);
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;

        m_Animator.enabled = false;
        EnableRagdoll(true);

        GetComponent<Collider>().enabled = false;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerObj.GetComponent<PlayerController>().Heal(10f);
        }
        Destroy(gameObject, 5f);
        
    }
   
}
