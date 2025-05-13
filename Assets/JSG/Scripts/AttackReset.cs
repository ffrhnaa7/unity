using StarterAssets;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Diagnostics;

public class AttackReset : StateMachineBehaviour
{
    [SerializeField]
    private string _triggerName;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerController pc = animator.GetComponent<PlayerController>();
        pc.DisableBehavior(PlayerController.EPlayerBehavior.Move);
        pc.DisableBehavior(PlayerController.EPlayerBehavior.Dodge);
        Debug.Log("Disable Move");
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.ResetTrigger(_triggerName);
        //PlayerController pc = animator.GetComponent<PlayerController>();
        //pc.EnableBehavior(PlayerController.EPlayerBehavior.Move);
        //pc.EnableBehavior(PlayerController.EPlayerBehavior.Dodge);
        //Debug.Log("Enable Move");
    }

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        //PlayerController pc = animator.GetComponent<PlayerController>();
        //pc.DisableBehavior(PlayerController.EPlayerBehavior.Move);
        //pc.DisableBehavior(PlayerController.EPlayerBehavior.Dodge);
        //Debug.Log("Disable Move");
    }

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        animator.ResetTrigger(_triggerName);
        PlayerController pc = animator.GetComponent<PlayerController>();
        pc.EnableBehavior(PlayerController.EPlayerBehavior.Move);
        pc.EnableBehavior(PlayerController.EPlayerBehavior.Dodge);
        Debug.Log("Enable Move");
    }

    
}
