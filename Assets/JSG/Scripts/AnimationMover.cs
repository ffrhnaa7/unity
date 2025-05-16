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
                Vector3 worldMoveDir = transform.TransformDirection(_autoMoveDirection.normalized);
                _controller.Move(worldMoveDir * _autoMoveSpeed * Time.deltaTime);
            }
        }
    }
    static Dictionary<string, float> ParseMoveParams(string param)
    {
        Dictionary<string, float> parsed = new Dictionary<string, float>();
        string key = "";
        string value = "";
        bool keyProcessing = true;
        for(int i = 0; i < param.Length; i++)
        {
            if (param[i] == ';')
            {
                parsed[key] = float.Parse(value);

                key = "";
                value = "";
                keyProcessing = true;
            }
            else if (param[i] == '=')
            {
                keyProcessing = false;
            }
            else
            {
                char c = char.ToLower(param[i]);
                if (keyProcessing)
                {
                    key += c;
                }
                else
                {
                    value += c;
                }
            }
        }
        return parsed;
    }
    // 애니메이션 이벤트로 호출됨
    public void StartAutoMove(string param)
    {
        //Debug.Log($"StartAutoMove, {param}");
        Dictionary<string, float> parsed = ParseMoveParams(param);
        float speed = parsed["speed"];
        float dirX = parsed["x"];
        float dirZ = parsed["z"];

        _autoMoveSpeed = speed;
        _autoMoveDirection = new Vector3(dirX, 0, dirZ);
        _isAutoMoving = true;
    }

    public void StopAutoMove()
    {
        //Debug.Log("StopAutoMove");
        _isAutoMoving = false;
        _controller.Move(Vector3.zero);
    }
}
