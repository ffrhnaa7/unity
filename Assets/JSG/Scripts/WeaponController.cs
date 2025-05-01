<<<<<<< HEAD
=======
using System.Collections;
>>>>>>> team/main
using UnityEngine;

public class WeaponController : MonoBehaviour
{
<<<<<<< HEAD
    public GameObject _weapon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_weapon == null) Debug.LogError("_weapon is null");
    }

    // Update is called once per frame
    void Update()
    {
        
=======
    public GameObject Player;
    public TrailRenderer Trail;
    private CapsuleCollider _meleeArea;

    [SerializeField]
    private float _damage = 10;
    private float _dalayTime = 0.25f;
    private void Awake()
    {
        if (Player == null) Debug.LogError("Player is null");

        _meleeArea = GetComponent<CapsuleCollider>();
        Debug.Log($"{Trail}");
        Trail.enabled = false;
        _meleeArea.enabled = false;
>>>>>>> team/main
    }

    public void ActiveWeapon(bool active)
    {
<<<<<<< HEAD
        _weapon.SetActive(active);
=======
        gameObject.SetActive(active);
    }

    public void Use(float Time = 0.25f)
    {
        Debug.Log("Use");
        _dalayTime = Time;

        StopCoroutine("Swing");
        StartCoroutine("Swing");
    }

    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.25f);
        Trail.enabled = true;
        _meleeArea.enabled = true;
        Debug.Log("Swing, before WaitForSeconds");
        yield return new WaitForSeconds(_dalayTime);
        Debug.Log("Swing, After WaitForSeconds");
        Trail.enabled = false;
        _meleeArea.enabled = false;
    }

    public void WeaponEnable(bool Enable)
    {
        Trail.enabled = Enable;
        _meleeArea.enabled = Enable;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IEnemy Enemy))
        {
            Enemy.GetDamage(_damage);
        }
>>>>>>> team/main
    }
}
