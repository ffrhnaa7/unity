using UnityEngine;
using StarterAssets;

public class DetectingPlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Damaged");
            // 플레이어 피 깎기
            PlayerController pc = other.GetComponent<PlayerController>();
            pc.GetDamage(10);
        }
    }
}
