using UnityEngine;

public class GoblinWeaponHandler : MonoBehaviour
{
    [SerializeField] private GameObject swordPrefab;      // Drag OM_Sword_R1 prefab here
    [SerializeField] private Transform handTransform;      // Assign BN_RightHand in Inspector

    private GameObject swordInstance;

    void Start()
    {
        WeaponEnable(true);  // Enable sword on start (optional)
    }

    public void WeaponEnable(bool active)
    {
        if (swordInstance != null)
        {
            swordInstance.SetActive(active);
            return;
        }

        if (swordPrefab != null && handTransform != null)
        {
            swordInstance = Instantiate(swordPrefab, handTransform);
            swordInstance.SetActive(active);
        }
        else
        {
            Debug.LogWarning("GoblinWeaponHandler: Missing swordPrefab or handTransform!");
        }
    }


}
