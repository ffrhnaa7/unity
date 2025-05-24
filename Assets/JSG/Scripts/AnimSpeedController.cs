using JetBrains.Annotations;
using UnityEngine;

public class AnimSpeedController : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    public void SetAnimSpeed(float speed)
    {
        _animator.speed = speed;
    }

    public void SetAnimSpeedNormal()
    {
        _animator.speed = 1.0f;
    }
}
