using UnityEngine;

public class DetectingPlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Damaged");
            // �÷��̾� �� ���

        }
    }
}
