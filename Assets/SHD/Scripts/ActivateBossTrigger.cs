using UnityEngine;

public class ActivateBossTrigger : MonoBehaviour
{
    public GameObject bossTrigger;
    public GameObject Rock;

    public AudioSource audioSource;
    public AudioClip audioClip;

    private void Awake()
    {
        //gameObject.GetComponent<Renderer>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bossTrigger.SetActive(true);
            Rock.SetActive(false);
            audioSource.clip = audioClip;
            audioSource.Play();
            Destroy(gameObject);
        }
    }
}
