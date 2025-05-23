using DG.Tweening;
using StarterAssets;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class AnimationMover : MonoBehaviour
{
    private float _autoMoveSpeed = 5f;
    private Vector3 _autoMoveDirection = Vector3.zero;

    private CharacterController _controller;
    private PlayerController _pc;
    private HitStop _hitStop;
    private bool _isAutoMoving = false;
    private Vector3 _worldMoveDir;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _hitStop = GetComponent<HitStop>();
        _pc = GetComponent<PlayerController>();
    }
    void Update()
    {
        if (_isAutoMoving)
        {
            if (!_hitStop.isHitStop())
            {
                _controller.Move(_worldMoveDir * _autoMoveSpeed * Time.deltaTime);
            }
        }
    }
    
    // �ִϸ��̼� �̺�Ʈ�� ȣ���
    public void StartAutoMove(string param)
    {
        //Debug.Log($"StartAutoMove, {param}");
        Dictionary<string, float> parsed = ParamsParser.ParseEventParams(param);
        float speed = parsed["speed"];
        float dirX = parsed["x"];
        float dirZ = parsed["z"];

        _autoMoveSpeed = speed;
        _autoMoveDirection = new Vector3(dirX, 0, dirZ);
        _worldMoveDir = transform.TransformDirection(_autoMoveDirection.normalized);
        _isAutoMoving = true;
    }

    public void StopAutoMove()
    {
        //Debug.Log("StopAutoMove");
        _isAutoMoving = false;
        _controller.Move(Vector3.zero);
    }

    public void StartAutoRotate(string param)
    {
        Dictionary<string, float> parsed = ParamsParser.ParseEventParams(param);
        float duration = parsed["duration"];
        float x = parsed["x"];
        float y = parsed["y"];
        float z = parsed["z"];
        transform.DORotate(new Vector3(x, y, z), duration, RotateMode.WorldAxisAdd);
    }

    public void StopAutoRotate()
    {
        DOTween.Kill(transform);
    }
}
