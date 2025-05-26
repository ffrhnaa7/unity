using UnityEngine;

public class ActivateBossTrigger : MonoBehaviour
{
    public GameObject bossTrigger;
    private void Awake()
    {
        //gameObject.GetComponent<Renderer>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bossTrigger.SetActive(true);
            Destroy(gameObject);
        }
    }
}
