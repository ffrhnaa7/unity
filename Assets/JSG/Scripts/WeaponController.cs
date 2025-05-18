using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject Player;
    [SerializeField]
    private TrailRenderer _trail;
    private CapsuleCollider _meleeArea;
    private HashSet<IEnemy> _damagedTargets = new HashSet<IEnemy>();
    private CameraShaker _cameraShaker;
    [SerializeField]
    private float _damage = 10;

    public Gradient OriginalGradient;
    public Gradient HitGradient;

    private HitStop _hitStop;
    private void Awake()
    {
        if (Player == null) Debug.LogError("Player is null");

        _hitStop = Player.GetComponent<HitStop>();
        if (_hitStop == null)
        {
            Debug.LogError("Failed to find HitStop Script");
        }
        _cameraShaker = Player.GetComponent<CameraShaker>();
        _meleeArea = GetComponent<CapsuleCollider>();
        _trail.enabled = false;
        _meleeArea.enabled = false;
    }

    public void SetSwordColliderScale(float Scale)
    {
        //collider.localScale *= Scale;
        CapsuleCollider collider = GetComponent<CapsuleCollider>();
        collider.radius *= Scale;
    }


    public void WeaponEnable(bool Enable)
    {
        if (!Enable)
        {
            _damagedTargets.Clear();
        }
        ChangeTrailColor(OriginalGradient);
        _trail.enabled = Enable;
        _meleeArea.enabled = Enable;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IEnemy Enemy) && !_damagedTargets.Contains(Enemy))
        {
            //Debug.Log("Weapon Collider Detected Enemy");
            _damagedTargets.Add(Enemy);
            Enemy.GetDamage(_damage);
            _hitStop.DoHitStop();
            _cameraShaker.Shake(.05f, new Vector3(0.15f, 0.15f, 0));
            Invoke(nameof(ChangeTrailColorHitGradient), 0.15f);
        }
    }

    private void ChangeTrailColor(Gradient gradient)
    {
        _trail.colorGradient = gradient;
    }

    private void ChangeTrailColorHitGradient()
    {
        _trail.colorGradient = HitGradient;
    }
}
