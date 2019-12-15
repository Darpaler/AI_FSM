using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeadBehavior : StateMachineBehaviour
{
    // Variables
    private Animator animator;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.animator = animator;

        GameManager.instance.actionTwoButton.GetComponentInChildren<TextMeshProUGUI>().text = "Bury";

        GameManager.instance.virtualPet.ResetUI();
        GameManager.instance.actionOneButton.gameObject.SetActive(false);
        GameManager.instance.actionTwoButton.onClick.AddListener(Bury);
        GameManager.instance.actionThreeButton.gameObject.SetActive(false);
        GameManager.instance.pass.gameObject.SetActive(false);

        GameManager.instance.virtualPet.GetComponent<SpriteRenderer>().color = Color.gray;

        animator.SetBool("dead", true);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.actionOneButton.gameObject.SetActive(true);
        GameManager.instance.actionThreeButton.gameObject.SetActive(true);
        GameManager.instance.pass.gameObject.SetActive(true);
        GameManager.instance.virtualPet.GetComponent<SpriteRenderer>().color = Color.white;

        animator.SetBool("dead", false);
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

    void Bury()
    {
        animator.SetInteger("age", 0);
    }
}
