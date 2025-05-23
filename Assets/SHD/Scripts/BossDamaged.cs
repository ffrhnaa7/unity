using UnityEngine;

public class BossDamaged : MonoBehaviour, IEnemy
{
    BossAI bossAI;

    void Start()
    {
        bossAI = GetComponentInParent<BossAI>();
    }

    public void GetDamage(float damage)
    {
        Debug.Log("BossDamaged");
        bossAI.bossCurrentHp -= damage;
    }
}
