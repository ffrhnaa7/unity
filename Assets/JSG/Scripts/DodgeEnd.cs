using StarterAssets;
using UnityEngine;
using UnityEngine.Diagnostics;

public class DodgeEnd : StateMachineBehaviour
{
    private CharacterController _controller;
    private float _dodgeSpeed;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _controller = animator.GetComponent<CharacterController>();
        _dodgeSpeed = animator.GetComponent<PlayerController>().DodgeSpeed;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //_controller.Move(_controller.transform.forward * (_dodgeSpeed * Time.deltaTime));
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CharacterController controller = animator.GetComponent<CharacterController>();
        PlayerController pc = animator.GetComponent<PlayerController>();
        controller.Move(Vector3.zero);
        pc.DodgeEnd();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
