﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HappyBehavior : StateMachineBehaviour
{
    // Variables
    private Animator animator;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        this.animator = animator;
        GameManager.instance.actionOneButton.GetComponentInChildren<TextMeshProUGUI>().text = "Play";
        GameManager.instance.actionTwoButton.GetComponentInChildren<TextMeshProUGUI>().text = "Feed";
        GameManager.instance.actionThreeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Send to bed";

        GameManager.instance.virtualPet.ResetUI();
        GameManager.instance.actionOneButton.onClick.AddListener(Play);
        GameManager.instance.actionTwoButton.onClick.AddListener(Feed);
        GameManager.instance.actionThreeButton.onClick.AddListener(Sleep);
        GameManager.instance.pass.onClick.AddListener(Pass);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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

    void Feed()
    {
        // Restore hunger
        animator.SetFloat("hunger", animator.GetFloat("hunger") + 4);              
        
        // If over fed
        if (animator.GetFloat("hunger") > 10)
        {
            // Max hunger
            animator.SetFloat("hunger", 10);
            // Lower affection
            animator.SetFloat("affection", Mathf.Clamp(animator.GetFloat("affection") - 3, 0, 10));
        }
        GameManager.instance.virtualPet.EndTurn();
    }

    void Play()
    {
        animator.SetFloat("affection", Mathf.Clamp(animator.GetFloat("affection") + 1, 0, 10));
        animator.SetFloat("energy", animator.GetFloat("energy") - 2);
        GameManager.instance.virtualPet.EndTurn();
    }

    void Sleep()
    {
        GameManager.instance.virtualPet.Sleep();
    }

    void Pass()
    {
        animator.SetFloat("energy", animator.GetFloat("energy") - 1);
        GameManager.instance.virtualPet.EndTurn();
    }
}
