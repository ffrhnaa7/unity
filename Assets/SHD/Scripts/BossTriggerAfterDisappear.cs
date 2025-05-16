using UnityEngine;

public class BossTriggerAfterDisappear : MonoBehaviour
{
    public Animator bossAnim;
    public GameObject bossPrefab;
    public BossAI bossAI;

    private void Awake()
    {
        //gameObject.GetComponent<Renderer>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bossAI.ChangeRendererModeToOpaque();
            bossAI.bossHp = 50;
            bossPrefab.SetActive(true);
            bossAnim.SetTrigger("Trigger");
            Debug.Log("Boss Appear");
            Destroy(gameObject);
        }
    }
}
