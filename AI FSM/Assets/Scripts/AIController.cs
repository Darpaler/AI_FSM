using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class AIController : MonoBehaviour
{

    // Variables
    private Animator animator;
    private int turnsLeft;
    [SerializeField]
    private TextMeshProUGUI dayUI;
    [SerializeField]
    private TextMeshProUGUI turnsUI;

    public TextMeshProUGUI hungerUI;
    public TextMeshProUGUI energyUI;
    public TextMeshProUGUI affectionUI;
    public TextMeshProUGUI entertainmentUI;
    public TextMeshProUGUI healthUI;

    [SerializeField]
    private Button optionOneButton;
    private TextMeshProUGUI optionOneText;
    [SerializeField]
    private Button optionTwoButton;
    private TextMeshProUGUI optionTwoText;
    [SerializeField]
    private Button optionThreeButton;
    private TextMeshProUGUI optionThreeText;


    private int defaultAge;
    private float defaultHunger;
    private float defaultEnergy;
    private bool defaultDayTime;
    private float defaultAffection;
    private float defaultEntertainment;
    private float defaultHealth;



    // Start is called before the first frame update
    void Start()
    {
        // Get Components
        animator = GetComponent<Animator>();
        turnsLeft = GameManager.instance.turnsPerDay;
        optionOneText = optionOneButton.GetComponentInChildren<TextMeshProUGUI>();
        optionTwoText = optionTwoButton.GetComponentInChildren<TextMeshProUGUI>();
        optionThreeText = optionThreeButton.GetComponentInChildren<TextMeshProUGUI>();

        SetUI();

        // Default Variables
        defaultAge = animator.GetInteger("age");
        defaultHunger = animator.GetFloat("hunger");
        defaultEnergy = animator.GetFloat("energy");
        defaultDayTime = animator.GetBool("dayTime");
        defaultAffection = animator.GetFloat("affection");
        defaultEntertainment = animator.GetFloat("entertainment");
        defaultHealth = animator.GetFloat("health");
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

    private void ResetOptions()
    {
        optionOneButton.onClick.RemoveAllListeners();
        optionTwoButton.onClick.RemoveAllListeners();
        optionThreeButton.onClick.RemoveAllListeners();
    }

    public void SetUI()
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

        hungerUI.text = "Hunger: " + animator.GetFloat("hunger") + "/10";
        energyUI.text = "Energy: " + animator.GetFloat("energy") + "/10";
        affectionUI.text = "Affection: " + animator.GetFloat("affection") + "/10";
        entertainmentUI.text = "Entertainment: " + animator.GetFloat("entertainment") + "/10";
        healthUI.text = "Health: " + animator.GetFloat("health") + "/10";

    }

    private void ToggleOptions()
    {
        GameManager.instance.actionOneButton.gameObject.SetActive(!GameManager.instance.actionOneButton.gameObject.activeSelf);
        GameManager.instance.actionTwoButton.gameObject.SetActive(!GameManager.instance.actionTwoButton.gameObject.activeSelf);
        GameManager.instance.actionThreeButton.gameObject.SetActive(!GameManager.instance.actionThreeButton.gameObject.activeSelf);
        GameManager.instance.pass.gameObject.SetActive(!GameManager.instance.pass.gameObject.activeSelf);
        optionOneButton.gameObject.SetActive(!optionOneButton.gameObject.activeSelf);
        optionTwoButton.gameObject.SetActive(!optionTwoButton.gameObject.activeSelf);
        optionThreeButton.gameObject.SetActive(!optionThreeButton.gameObject.activeSelf);
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
        if (animator.GetBool("nocturnal"))
        {
            turnsLeft = (GameManager.instance.turnsPerDay / 2) + 1;
        }
        else
        {
            turnsLeft = 1;
        }
        EndTurn();
    }

    public void Feed()
    {
        optionOneText.text = "Sour";
        optionTwoText.text = "Sweet";
        optionThreeText.text = "Spicy";

        ResetOptions();
        optionOneButton.onClick.AddListener(FeedSour);
        optionTwoButton.onClick.AddListener(FeedSweet);
        optionThreeButton.onClick.AddListener(FeedSpicy);
        ToggleOptions();

    }

    private void FeedSour()
    {
        if (animator.GetInteger("nature") == 1)
        {
            animator.SetFloat("affection", Mathf.Clamp(animator.GetFloat("affection") + 1, 0, 10));
        }
        else if (animator.GetInteger("nature") == 3)
        {
            animator.SetFloat("affection", Mathf.Clamp(animator.GetFloat("affection") - 1, 0, 10));
        }
        ToggleOptions();
    }
    private void FeedSweet()
    {
        if (animator.GetInteger("nature") == 2)
        {
            animator.SetFloat("affection", Mathf.Clamp(animator.GetFloat("affection") + 1, 0, 10));
        }
        else if (animator.GetInteger("nature") == 1)
        {
            animator.SetFloat("affection", Mathf.Clamp(animator.GetFloat("affection") - 1, 0, 10));
        }
        ToggleOptions();
    }
    private void FeedSpicy()
    {
        if (animator.GetInteger("nature") == 3)
        {
            animator.SetFloat("affection", Mathf.Clamp(animator.GetFloat("affection") + 1, 0, 10));
        }
        else if (animator.GetInteger("nature") == 2)
        {
            animator.SetFloat("affection", Mathf.Clamp(animator.GetFloat("affection") - 1, 0, 10));
        }
        ToggleOptions();
    }



    public void ResetStats()
    {
        animator.SetInteger("age", defaultAge);
        animator.SetFloat("hunger", defaultHunger);
        animator.SetFloat("energy", defaultEnergy);
        animator.SetBool("dayTime", defaultDayTime);
        animator.SetFloat("affection", defaultAffection);
        animator.SetFloat("entertainment", defaultEntertainment);
        animator.SetFloat("health", defaultHealth);
    }

}
