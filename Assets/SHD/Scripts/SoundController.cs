using UnityEngine;

public class SoundController : MonoBehaviour
{
    BossAI bossAI;

    public AudioSource audioSource;
    public AudioClip emergenceSound;
    public AudioClip attack_1Sound;
    public AudioClip attack_2Sound;
    public AudioClip fireAttackSound;
    public AudioClip footSoundLeft;
    public AudioClip footSoundRight;
    public AudioClip deadSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bossAI = GetComponentInParent<BossAI>();
        audioSource = GetComponent<AudioSource>();
    }

    public void AttackSound_1()
    {
        audioSource.clip = attack_1Sound;
        audioSource.pitch = 1.2f;
        audioSource.time = 0f;
        audioSource.Play();
    }

    public void AttackSound_2()
    {
        audioSource.clip = attack_2Sound;
        audioSource.pitch = 1.25f;
        audioSource.time = 0f;
        audioSource.Play();
    }

    public void FireAttackSoundDelay(float delay)
    {
        Invoke("FireAttackSound", delay);
    }

    private void FireAttackSound()
    {
        audioSource.clip = fireAttackSound;
        audioSource.pitch = 1.0f;
        audioSource.time = 2.3f;
        audioSource.Play();
    }

    public void FootStepLeftSound()
    {
        audioSource.PlayOneShot(footSoundLeft);
    }

    public void FootStepRightSound()
    {
        audioSource.PlayOneShot(footSoundRight);
    }

    public void DeathSound()
    {
        audioSource.clip = deadSound;
        audioSource.pitch = 0.85f;
        audioSource.time = 0f;
        audioSource.Play();
    }
}
