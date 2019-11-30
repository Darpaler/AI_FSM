using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EggBehavior : StateMachineBehaviour
{
    // Variables
    private Animator animator;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.animator = animator;

        GameManager.instance.actionOneButton.GetComponentInChildren<TextMeshProUGUI>().text = "Shake";
        GameManager.instance.actionTwoButton.GetComponentInChildren<TextMeshProUGUI>().text = "Throw At Wall";
        GameManager.instance.actionThreeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Wait Patiently";
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.actionOneButton.onClick.AddListener(Shake);
        GameManager.instance.actionTwoButton.onClick.AddListener(Throw);
        GameManager.instance.actionThreeButton.onClick.AddListener(Wait);
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

    void Shake()
    {
        animator.SetInteger("species", (int) GameManager.Species.Snake);
        animator.SetBool("nocturnal", false);
        GameManager.instance.virtualPet.GetComponent<SpriteRenderer>().sprite = GameManager.instance.snakeSprite;
        animator.SetInteger("age", 1);
    }

    void Throw()
    {
        animator.SetInteger("species", (int)GameManager.Species.Sloth);
        GameManager.instance.virtualPet.GetComponent<SpriteRenderer>().sprite = GameManager.instance.slothSprite;
        animator.SetInteger("age", 1);
    }

    void Wait()
    {
        animator.SetInteger("species", (int)GameManager.Species.Owl);
        animator.SetBool("nocturnal", true);
        GameManager.instance.virtualPet.GetComponent<SpriteRenderer>().sprite = GameManager.instance.owlSprite;
        animator.SetInteger("age", 1);
    }
}
