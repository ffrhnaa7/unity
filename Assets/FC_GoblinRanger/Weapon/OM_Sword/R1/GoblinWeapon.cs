using UnityEngine;

public class GoblinWeaponHandler : MonoBehaviour
{
    [Header("Assign the child sword GameObject manually in Inspector")]
    [SerializeField] private GameObject swordInstance;

    private Collider swordCollider;

    private void Awake()
    {
        if (swordInstance == null)
        {
            Debug.LogError("❌ GoblinWeaponHandler: Sword instance not assigned.");
            enabled = false;
            return;
        }

        // Try to find a collider on the sword
        swordCollider = swordInstance.GetComponent<Collider>();
        if (swordCollider == null)
        {
            Debug.LogError("❌ GoblinWeaponHandler: No Collider found on the sword.");
            enabled = false;
            return;
        }

        // Ensure sword is visible, but collider is off by default
        swordInstance.SetActive(true);
        swordCollider.enabled = false;
    }

    /// <summary>
    /// Called by Animation Event when attack begins
    /// </summary>
    public void EnableWeaponCollider()
    {
        if (swordCollider != null)
        {
            swordCollider.enabled = true;
            Debug.Log("✅ Sword collider enabled");
        }
    }

    /// <summary>
    /// Called by Animation Event when attack ends
    /// </summary>
    public void DisableWeaponCollider()
    {
        if (swordCollider != null)
        {
            swordCollider.enabled = false;
            Debug.Log("🛑 Sword collider disabled");
        }
    }
}
