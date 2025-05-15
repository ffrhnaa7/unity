using UnityEngine;

public class GoblinWeaponCollider : MonoBehaviour
{
    public float damage = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("⚔️ Sword hit player");
            other.SendMessage("GetDamage", damage, SendMessageOptions.DontRequireReceiver);

            // prevent multiple hits if necessary
            GetComponent<Collider>().enabled = false;
        }
    }
}
