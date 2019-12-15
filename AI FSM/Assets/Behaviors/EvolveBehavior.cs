using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolveBehavior : StateMachineBehaviour
{
    // Variables
    private Animator animator;
    [SerializeField]
    private float timeToEvolve = 5f;
    private float currentTime = 0;
    [SerializeField]
    private float sizeIncrease = 3f;
    private bool grew1;
    private bool grew2;
    private bool grew3;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.animator = animator;
        GameManager.instance.virtualPet.ResetUI();
        GameManager.instance.actionOneButton.gameObject.SetActive(false);
        GameManager.instance.actionTwoButton.gameObject.SetActive(false);
        GameManager.instance.actionThreeButton.gameObject.SetActive(false);
        GameManager.instance.pass.gameObject.SetActive(false);

        currentTime = timeToEvolve;

        animator.SetBool("evolving", true);

        grew1 = false;
        grew2 = false;
        grew3 = false;
}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentTime -= Time.deltaTime;
        if(currentTime <= timeToEvolve * 0.66 && !grew1)
        {
            GameManager.instance.virtualPet.gameObject.transform.localScale += new Vector3 (sizeIncrease / 3, sizeIncrease / 3, sizeIncrease / 3);
            grew1 = true;
        }
        else if (currentTime <= timeToEvolve * 0.33 && !grew2)
        {
            GameManager.instance.virtualPet.gameObject.transform.localScale += new Vector3(sizeIncrease / 3, sizeIncrease / 3, sizeIncrease / 3);
            grew2 = true;
        }
        else if (currentTime <= 0 && !grew3)
        {
            GameManager.instance.virtualPet.gameObject.transform.localScale += new Vector3(sizeIncrease / 3, sizeIncrease / 3, sizeIncrease / 3);
            animator.SetInteger("age", 6);
            grew3 = true;
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.actionOneButton.gameObject.SetActive(true);
        GameManager.instance.actionTwoButton.gameObject.SetActive(true);
        GameManager.instance.actionThreeButton.gameObject.SetActive(true);
        GameManager.instance.pass.gameObject.SetActive(true);
        animator.SetBool("evolving", false);
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
