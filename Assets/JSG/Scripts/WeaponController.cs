using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject _weapon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_weapon == null) Debug.LogError("_weapon is null");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActiveWeapon(bool active)
    {
        _weapon.SetActive(active);
    }
}
