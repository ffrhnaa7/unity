using UnityEngine;

public class GoblinLocation  : MonoBehaviour
{
    public GameObject goblinPrefab;
    public Transform[] spawnPoints;

    void Start()
    {
        foreach (Transform point in spawnPoints)
        {
            Instantiate(goblinPrefab, point.position, point.rotation);
        }
    }
}
