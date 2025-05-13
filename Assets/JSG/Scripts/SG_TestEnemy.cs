using UnityEngine;

public class SG_TestEnemy : MonoBehaviour, IEnemy
{
    public float MaxHP = 100;
    private float _hp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _hp = MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetDamage(float Damage)
    {
        _hp = Mathf.Max(0, _hp - Damage);
        Debug.Log($"enemy Get Damage {Damage}");
    }
}
