using UnityEngine;
using StarterAssets;

public class Enemy01Weapon : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Collider>().enabled = false;
    }
    // 적 Enemy01 Weapon에 닿으면 플레이어가 공격 당하도록 하는 코드
    public float damage = 1f;
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[TestTrigger] Trigger entered by: {other.name}");

        if (other.CompareTag("Player"))
        {
            Debug.Log("[Enemy01Weapon] Detected player!");

            if (other.TryGetComponent(out PlayerController player))
            {
                Debug.Log("[Enemy01Weapon] PlayerController found, applying damage.");
                player.GetDamage(damage);
            }
        }
    }

    public void EnableWeapon(bool enable)
    {
        var collider = GetComponent<Collider>();
        collider.enabled = enable;
        Debug.Log($"[Enemy01Weapon] Collider enabled = {collider.enabled}");
    }
    
    private void OnDrawGizmos()
    {
        var col = GetComponent<BoxCollider>();
        if (col && col.enabled)
        {
            Gizmos.color = Color.yellow;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(col.center, col.size);
        }
    }

    
}
