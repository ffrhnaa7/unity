using StarterAssets;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public Animator bossAnim;
    public GameObject bossPrefab;
    public GameObject bossTrigger;
    public Transform bossTransform;

    private void Awake()
    {
        //gameObject.GetComponent<Renderer>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            bossPrefab.SetActive(true);
            bossTrigger.SetActive(true);
            bossAnim.SetTrigger("Trigger");
            Debug.Log("Boss Appear");
            Destroy(gameObject);

            // add by ½Â°Ç
            PlayerController pc = other.GetComponent<PlayerController>();
            pc.BossAppear(bossTransform);
            //
        }
    }
}
