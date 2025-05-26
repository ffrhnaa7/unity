using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    // Enemy01_AI를 외부에서 연결해줄 변수
    public Enemy01_AI enemyAI;

    // 애니메이션 이벤트에서 호출할 함수
    public void EnableWeaponTrue() {
        if (enemyAI != null) enemyAI.EnableWeaponTrue();
    }
    public void EnableWeaponFalse() {
        if (enemyAI != null) enemyAI.EnableWeaponFalse();
    }
    
    public AudioSource audioSource;
    public AudioClip attackSound;

    public void PlayAttackSound()
    {
        if (audioSource != null && attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }
    }
}