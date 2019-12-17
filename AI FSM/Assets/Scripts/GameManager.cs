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
    public Button pass;

    [Header("Animal Settings")]
    public AIController virtualPet;
    public Sprite snakeSprite;
    public Sprite owlSprite;
    public Sprite slothSprite;
    public Sprite eggSprite;
    public enum Species {Snake, Owl, Sloth}

    [Header("World Settings")]
    public int turnsPerDay;

    [Header("Pathfinding Settings")]
    public GameObject nodesParent;
    public List<Node> nodes;
    public int numberOfRows = 12;
    public int numberOfCols = 8;
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

    void ClearAllNodes()
    {
        foreach (Node node in GameManager.instance.nodes)
        {
            node.connections.Clear();
        }
    }

    int Index2D(int x, int y)
    {
        if (x < 0 || x > numberOfCols || y < 0 || y > numberOfRows)
        {
            Debug.LogError("Error: (" + x + "," + y + ") is out of range.");
            return -1;
        }

        return (x + y * numberOfCols);
    }

    public void ConnectAllNodes()
    {
        ClearAllNodes();

        for (int row = 0; row < numberOfRows; row++)
        {
            // If not in the first row
            if (row > 0)
            {
                // Add all open tops                
                for (int col = 0; col < numberOfCols; col++)
                {
                    if (!nodes[Index2D(col, row - 1)].isBlocked)
                    {
                        nodes[Index2D(col, row)].AddUniqueLinkTo(nodes[Index2D(col, row - 1)]);
                    }
                }
            }

            // If not in the last row, add all open bottoms
            if (row < numberOfRows - 1)
            {
                // Add all open bottoms                
                for (int col = 0; col < numberOfCols; col++)
                {
                    if (!nodes[Index2D(col, row + 1)].isBlocked)
                    {
                        nodes[Index2D(col, row)].AddUniqueLinkTo(nodes[Index2D(col, row + 1)]);
                    }
                }
            }
        }

        for (int col = 0; col < numberOfCols; col++)
        {
            // If not in the first col, add all open lefts
            if (col > 0)
            {
                // Add all open lefts                
                for (int row = 0; row < numberOfRows; row++)
                {
                    if (!nodes[Index2D(col - 1, row)].isBlocked)
                    {
                        nodes[Index2D(col, row)].AddUniqueLinkTo(nodes[Index2D(col - 1, row)]);
                    }
                }
            }

            // If not in the last col, add all open rights
            if (col < numberOfCols - 1)
            {
                // Add all open lefts                
                for (int row = 0; row < numberOfRows; row++)
                {
                    if (!nodes[Index2D(col + 1, row)].isBlocked)
                    {
                        nodes[Index2D(col, row)].AddUniqueLinkTo(nodes[Index2D(col + 1, row)]);
                    }
                }
            }

        }

    }

    public void SetUpMaze()
    {
        nodes.Clear();
        foreach (Node node in nodesParent.GetComponentsInChildren<Node>())
        {
            nodes.Add(node);
        }
    }

}
