using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Variables
    [HideInInspector]
    public static GameManager instance;

    [Header("UI Settings")]
    public Button actionOneButton;
    public Button actionTwoButton;
    public Button actionThreeButton;

    [Header("Animal Settings")]
    public AIController virtualPet;
    public Sprite snakeSprite;
    public Sprite owlSprite;
    public Sprite slothSprite;
    public enum Species {Snake, Owl, Sloth}

    [Header("World Settings")]
    public int turnsPerDay;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("Error: Instance of Game Manager already exists. New Game Manager has been deleted.");
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
