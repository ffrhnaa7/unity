using UnityEngine;
using StarterAssets;

public class DetectingPlayer : MonoBehaviour
{
    BossAI bossAI;

    private void Awake()
    {
        bossAI = GetComponentInParent<BossAI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Damaged");
            // �÷��̾� �� ���
            PlayerController pc = other.GetComponent<PlayerController>();
            pc.GetDamage(bossAI.attackDamage);
        }
    }
}
