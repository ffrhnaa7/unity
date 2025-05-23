using DG.Tweening;
using System.Collections;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    public float StopDuration = 0.13f;

    private Animator _animator;
    private bool _stopping = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Failed To Animator");
        }
    }
    public void DoHitStop()
    {
        if (!_stopping)
            StartCoroutine(HitStopCoroutine());
    }
    public bool isHitStop()
    {
        return _stopping;
    }
    private IEnumerator HitStopCoroutine()
    {
        _stopping = true;
        float originalSpeed = _animator.speed;
        _animator.speed = 0f;
        transform.DOPause();
        yield return new WaitForSecondsRealtime(StopDuration); // ���� �ð��� ���絵 ������ �� �ް�

        transform.DORestart();
        _animator.speed = originalSpeed;
        _stopping = false;
    }
}
