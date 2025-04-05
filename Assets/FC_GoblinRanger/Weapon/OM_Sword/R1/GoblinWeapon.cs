using UnityEngine;

public class GoblinWeaponHandler : MonoBehaviour
{
    [SerializeField] private GameObject swordPrefab; // Drag OM_Sword_R1 prefab here

    private GameObject swordInstance;

    void Start()
    {
        EquipWeapon(true);
    }

    public void EquipWeapon(bool active)
    {
        if (swordInstance != null)
        {
            // If the sword already exists, just toggle its visibility
            swordInstance.SetActive(active);
            return;
        }

        if (swordPrefab != null)
        {
            // Instantiate the sword and parent it to the hand
       
            swordInstance.transform.localPosition = Vector3.zero;
            swordInstance.transform.localRotation = Quaternion.identity;
       

            // If you want to deactivate it on start
            swordInstance.SetActive(active);
        }
        else
        {
            Debug.LogWarning("GoblinWeaponHandler: Missing prefab or hand transform!");
        }
    }
}
