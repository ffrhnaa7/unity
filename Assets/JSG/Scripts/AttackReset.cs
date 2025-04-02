using StarterAssets;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Diagnostics;

public class AttackReset : StateMachineBehaviour
{
    [SerializeField]
    private string _triggerName;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //var pc = animator.GetComponent<PlayerController>();
        //Debug.Log("");
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(_triggerName);
    }
}
