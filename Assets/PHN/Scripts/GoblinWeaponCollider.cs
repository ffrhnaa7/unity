using UnityEngine;

public class GoblinWeaponCollider : MonoBehaviour
{
    public float damage = 10f;
public bool hasHitPlayer = false;

    private void OnEnable()
    {
        hasHitPlayer = false; // Reset each time weapon is enabled
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHitPlayer) return; // Prevent multiple hits in one swing

        if (other.CompareTag("Player"))
        {
            hasHitPlayer = true;

            // Send damage to player
            other.SendMessage("GetDamage", damage, SendMessageOptions.DontRequireReceiver);

            // Notify GoblinAI (optional)
            GoblinAI goblin = GetComponentInParent<GoblinAI>();
            if (goblin != null)
                goblin.OnPlayerHit();

            Debug.Log("✅ Goblin hit the player!");
        }
    }
}
