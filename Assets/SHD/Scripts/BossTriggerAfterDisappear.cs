using StarterAssets;
using UnityEngine;

public class BossTriggerAfterDisappear : MonoBehaviour
{
    public Animator bossAnim;
    public GameObject bossPrefab;
    public BossAI bossAI;
    public Transform bossTransform;

    private void Awake()
    {
        //gameObject.GetComponent<Renderer>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bossAI.ChangeRendererModeToOpaque();
            bossAI.bossCurrentHp = bossAI.bossMaxHp / 2;
            bossAI.attackDamage = 30.0f;
            bossAI.DisableFireAttackCollider();
            bossPrefab.SetActive(true);
            bossAnim.SetTrigger("Trigger");
            Debug.Log("Boss Appear");

            // add by ½Â°Ç
            PlayerController pc = other.GetComponent<PlayerController>();
            pc.BossAppear(bossTransform);
            //

            Destroy(gameObject);
        }
    }
}
