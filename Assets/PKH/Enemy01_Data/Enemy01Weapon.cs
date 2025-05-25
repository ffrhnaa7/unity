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
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent(out PlayerController player))
            {
                player.GetDamage(damage);
                Debug.Log($"[Enemy01Weapon] {gameObject.name} hit {other.name}!");

            }
        }
    }

    public void EnableWeapon(bool enable)
    {
        GetComponent<Collider>().enabled = enable;
    }
    
}
