using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject Player;
    public List<AudioClip> SlashSounds = new List<AudioClip>();
    [SerializeField]
    private TrailRenderer _trail;
    private CapsuleCollider _meleeArea;
    private HashSet<IEnemy> _damagedTargets = new HashSet<IEnemy>();
    private CameraShaker _cameraShaker;
    private AudioSource _audioSource;
    [SerializeField]
    private float _damage = 10;

    public Gradient OriginalGradient;
    public Gradient HitGradient;
    public GameObject AttackHitVFXObject;

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
        _audioSource = GetComponent<AudioSource>();
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
            PlaySlashSound();
            GameObject hitVFXObject = Instantiate(AttackHitVFXObject, transform);
            ParticleSystem hitVFX = hitVFXObject.GetComponent<ParticleSystem>();
            hitVFX.Play();
            Invoke(nameof(ChangeTrailColorHitGradient), 0.15f);
        }
    }

    private void PlaySlashSound()
    {
        int size = SlashSounds.Count;
        int rand_idx = Random.Range(0, size - 1);
        _audioSource.PlayOneShot(SlashSounds[rand_idx]);

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
