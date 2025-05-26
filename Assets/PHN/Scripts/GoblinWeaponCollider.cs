using UnityEngine;

public class GoblinWeaponCollider : MonoBehaviour
{
    public float damage = 10f;
    public bool hasHitPlayer = false;
    public float maxHitDistance = 1.5f; // Only apply damage if player is close

    private Transform goblinTransform;

    private void Awake()
    {
        goblinTransform = transform.root; // Goblin's main body
    }

    private void OnEnable()
    {
        hasHitPlayer = false; // Reset each time weapon is enabled
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHitPlayer) return;

        if (other.CompareTag("Player"))
        {
            float distance = Vector3.Distance(goblinTransform.position, other.transform.position);

            if (distance <= maxHitDistance)
            {
                hasHitPlayer = true;

                other.SendMessage("GetDamage", damage, SendMessageOptions.DontRequireReceiver);

                GoblinAI goblin = GetComponentInParent<GoblinAI>();
                if (goblin != null)
                    goblin.OnPlayerHit();

                Debug.Log("✅ Goblin hit the player!");
            }
            else
            {
                Debug.Log("❌ Player too far. No damage applied.");
            }
        }
    }
}
