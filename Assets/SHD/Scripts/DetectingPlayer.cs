using UnityEngine;
using StarterAssets;

public class DetectingPlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Damaged");
            // �÷��̾� �� ���
            PlayerController pc = other.GetComponent<PlayerController>();
            pc.GetDamage(10);
        }
    }
}
