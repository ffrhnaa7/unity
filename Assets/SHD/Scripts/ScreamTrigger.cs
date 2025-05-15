using UnityEngine;

public class ScreamTrigger : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip screamSound;

    private void Awake()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.clip = screamSound;
            audioSource.Play();
            Destroy(gameObject);
        }
    }
}
