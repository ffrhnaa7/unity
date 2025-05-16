using DG.Tweening;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    [SerializeField]
    private Transform _cameraRoot;
    private Tweener _shakingTweener;

    private void Awake()
    {
 //       _cameraRoot = GetComponent<Transform>();
    }
    public void Shake(float duration, Vector3 strength)
    {
        if (_shakingTweener == null)
        {
            _shakingTweener = _cameraRoot.transform.DOShakePosition(duration, strength);
        }
        else
        {
            _shakingTweener.Complete();
            _shakingTweener = _cameraRoot.transform.DOShakePosition(duration, strength);

        }
    }
}
