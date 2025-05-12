using UnityEngine;

public class GoblinWeaponCollider : MonoBehaviour
{
    public float damage = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.SendMessage("GetDamage", damage, SendMessageOptions.DontRequireReceiver);
        }
    }
}
