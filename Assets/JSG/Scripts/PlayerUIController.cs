using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    public Slider HPUI;
    private float _targetHP = 100;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPUI.value = _targetHP;
    }

    // Update is called once per frame
    void Update()
    {
        HPUI.value = Mathf.Lerp(HPUI.value, _targetHP, Time.deltaTime * 6);
    }

    public void SetHPUI(float MaxHP, float CurHP)
    {
        _targetHP = CurHP / MaxHP;
    }
}
