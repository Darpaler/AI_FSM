using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;

public class AIController : MonoBehaviour
{

    // Variables
    private Animator animator;
    private int turnsLeft;
    [SerializeField]
    private TextMeshProUGUI dayUI;
    [SerializeField]
    private TextMeshProUGUI turnsUI;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        turnsLeft = GameManager.instance.turnsPerDay;
        SetUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetUI()
    {
        GameManager.instance.actionOneButton.onClick.RemoveAllListeners();
        GameManager.instance.actionTwoButton.onClick.RemoveAllListeners();
        GameManager.instance.actionThreeButton.onClick.RemoveAllListeners();
        GameManager.instance.pass.onClick.RemoveAllListeners();
    }

    void SetUI()
    {
        turnsUI.text = "Turns Left: " + turnsLeft;
        if (animator.GetBool("dayTime"))
        {
            dayUI.text = "Day: " + animator.GetInteger("age");
        }
        else
        {
            dayUI.text = "Night: " + animator.GetInteger("age");
        }
    }

    public void EndTurn()
    {
        animator.SetFloat("hunger", animator.GetFloat("hunger") - 1);

        turnsLeft--;
        // If you used half of your turns
        if (turnsLeft <= GameManager.instance.turnsPerDay / 2)
        {
            // Change the time of day
            animator.SetBool("dayTime", false);
        }

        // Check if we're out of turns
        if (turnsLeft <= 0)
        {
            // Reset the turn count
            turnsLeft = GameManager.instance.turnsPerDay;
            // Change the time of day
            animator.SetBool("dayTime", true);
            // Increment age
            animator.SetInteger("age",animator.GetInteger("age") + 1);
        }
        
        // Set the UI
        SetUI();
        
        // If the pet ran out of energy, it passes out
        if(animator.GetFloat("energy") <= 0)
        {
            Sleep();
        }
    }
    
    public void Sleep()
    {
        // Refresh energy
        animator.SetFloat("energy", 10);
        
        // End the day
        turnsLeft = 1;
        EndTurn();
    }
}
