using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public Animator bossAnim;
    public GameObject bossPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            bossPrefab.SetActive(true);
            bossAnim.SetTrigger("Trigger");
            Debug.Log("Boss Appear");
        }
    }
}
