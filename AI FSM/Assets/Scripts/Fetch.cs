using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fetch : MonoBehaviour
{
    [SerializeField]
    private GameObject goalOne;
    [SerializeField]
    private GameObject goalTwo;
    [SerializeField]
    private GameObject goalThree;
    [SerializeField]
    public AStar ai;

    int n;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.nodesParent = gameObject;
        GameManager.instance.SetUpMaze();

        ai.GetComponent<SpriteRenderer>().sprite = GameManager.instance.virtualPet.GetComponent<SpriteRenderer>().sprite;

        n = Random.Range(1, 3);

        if(n == 1)
        {
            ai.targetNode = goalOne.GetComponent<Node>();
        }
        else if (n == 2)
        {
            ai.targetNode = goalTwo.GetComponent<Node>();
        }
        else
        {
            ai.targetNode = goalThree.GetComponent<Node>();
        }
    }

    private void Update()
    {
        if (GameManager.instance.virtualPet.isPlaying)
        {
            transform.parent.gameObject.BroadcastMessage("PlayFetch");
        }
    }

    public void ShowGoal()
    {
        if (n == 1)
        {
            goalOne.GetComponent<SpriteRenderer>().enabled = true;
        }
        else if (n == 2)
        {
            goalTwo.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            goalThree.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
