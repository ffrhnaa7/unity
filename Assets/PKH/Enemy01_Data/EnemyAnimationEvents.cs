using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    // Enemy01_AI를 외부에서 연결해줄 변수
    public Enemy01_AI enemyAI;

    // 애니메이션 이벤트에서 호출할 함수
    public void EnableWeaponTrue()
    {
        if (enemyAI != null)
        {
            enemyAI.EnableWeaponTrue();
        }
        else
        {
            Debug.LogWarning("[EnemyAnimationEvents] EnemyAI가 연결되지 않았습니다.");
        }
    }

    public void EnableWeaponFalse()
    {
        if (enemyAI != null)
        {
            enemyAI.EnableWeaponFalse();
        }
        else
        {
            Debug.LogWarning("[EnemyAnimationEvents] EnemyAI가 연결되지 않았습니다.");
        }
    }
}