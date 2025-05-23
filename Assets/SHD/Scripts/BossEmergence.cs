using UnityEngine;

public class BossEmergence : StateMachineBehaviour
{
    BossAI bossAI;
    bool isPage2 = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossAI = animator.GetComponent<BossAI>();
        
        // 보스 등장씬 소리 재생
        bossAI.soundController.audioSource.clip = bossAI.soundController.emergenceSound;
        bossAI.soundController.audioSource.PlayDelayed(0.75f);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 보스 등장 애니메이션 끝
        if(!isPage2)
        {
            bossAI.ActivateAI();
            isPage2 = true;
        }
        else
        {
            bossAI.ActivateAIForPage2();
        }
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
