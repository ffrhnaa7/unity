using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject Player;
    public TrailRenderer Trail;
    private CapsuleCollider _meleeArea;

    [SerializeField]
    private float _damage = 10;

    private HitStop _hitStop;
    private void Awake()
    {
        if (Player == null) Debug.LogError("Player is null");

        _hitStop = Player.GetComponent<HitStop>();
        if (_hitStop == null)
        {
            Debug.LogError("Failed to find HitStop Script");
        }

        _meleeArea = GetComponent<CapsuleCollider>();
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
            Debug.Log("Weapon Collider Detected Enemy");
            Enemy.GetDamage(_damage);
            _hitStop.DoHitStop();
        }
    }
}
