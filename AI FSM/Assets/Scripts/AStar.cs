using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AStar : MonoBehaviour
{
    [System.Serializable]
    public class NodeRecord
    {
        // Define what one "node record" is according to A*
        public Node node;
        public NodeConnection connection;
    }

    [Header("Variables")]
    public bool isRunning;
    public float speed = 3.5f;
    public int currentNodeInPath = 0;

    [Header("Lists for Pathfinding")]
    public Node startNode;
    public Node targetNode;
    public List<NodeConnection> path;
    public List<NodeRecord> openList;
    public List<NodeRecord> closedList;

    private bool reachedGoal = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.instance.ConnectAllNodes();
            GameManager.instance.nodesParent.GetComponent<Fetch>().ShowGoal();
            StartCoroutine("StartPathfinding");
        }
        if (isRunning)
        {
            Move();
        }
    }

    public IEnumerator StartPathfinding()
    {
        // Calculate the path
        yield return StartCoroutine("CalculatePath");

        // Add anything here needed to start running the race

        // When that is done, start moving
        isRunning = true;
        yield return null; // End of one frame draw
    }

    public IEnumerator CalculatePath()
    {
        // Initialize the Record
        NodeRecord startRecord = new NodeRecord();
        startRecord.node = startNode;
        startRecord.connection = null;

        // Initialize Lists and Variables
        openList = new List<NodeRecord>();
        closedList = new List<NodeRecord>(openList);
        NodeRecord current = null;
        NodeRecord endNodeRecord = null;

        // Start with just the start record in the open list
        openList.Add(startRecord);

        // NEXT FRAME
        yield return null;


        // Iterate through each node
        while (openList.Count > 0)
        {
            // Find the smallest element in the open list
            current = SmallestElement(openList);

            // If this is the goal, then terminate
            if (current.node == targetNode)
            {
                break;
            }

            // Otherwise, get its outgoing connections
            // Loop through each connection
            foreach (NodeConnection connection in current.node.connections)
            {
                // Skip if the node is closed
                // NOTE: SEE "ListContains(list, node)" helper function below
                if (ListContains(closedList, connection.toNode))
                {
                    continue;
                }

                // ... or if it is open 
                else if (ListContains(openList, connection.toNode))
                {
                    // Here we find the record in the open list corresponding to the endNode
                    endNodeRecord = FindInList(openList, connection.toNode);
                }
                else
                {
                    // Otherwie, we know we are at an unvisited node
                    // Make a record for it 
                    // NOTE: Use the same variable "endNodeRecord", so that we can update either this new one, OR the open one with the same line of code below
                    endNodeRecord = new NodeRecord();
                    endNodeRecord.node = connection.toNode;
                }

                // We're here if we need to update the node 
                // NOTE: If we had been closed, we would have "continue"'d out of here - OR - if we had been open, but worse, we would have "continue"'d out of here              
                endNodeRecord.connection = connection;

                // Add it to the open list
                // NOTE: Make sure it isn't already there
                if (!ListContains(openList, endNodeRecord.connection.toNode))
                {
                    openList.Add(endNodeRecord);
                }

                // (NOTE: Next frame -- so this process occurs over time)
                yield return null;
            }

            // We've looked at all the connections for the current node. 
            // Add it to closed list.
            closedList.Add(current);
            // Remove it from open list.
            openList.Remove(current);

            // (NOTE: Next frame -- so this process occurs over time)
            yield return null;
        }
        // We're here if we've either found the goal (NOTE: If we are the goal, we would "break" out to this point)
        //     OR we have no more nodes to search ( openList.Count <= 0 )
        // Find out which:
        if (current.node != targetNode)
        {
            // We ran out of nodes without finding a goal
            // Clear the path
            path.Clear();

            // Quit the function
            yield break;
        }
        // Compile the list of connections in the path
        path = new List<NodeConnection>();

        // Work back through the path, accumulating connections
        while (current.node != startNode)
        {
            //(NOTE: Add the connection to the path)
            path.Add(current.connection);
            //(NOTE: Move to the previous connection)
            current = FindInList(closedList, current.connection.fromNode);
        }

        // Reverse the path and save it
        path.Reverse();

        // Start at node zero
        currentNodeInPath = 0;

        // End of frame draw
        yield return null;
    }

    private void Move()
    {
        // Add any code that the AI needs to move
        // If we are not at the end of the goal
        if (currentNodeInPath < path.Count)
        {
            // Move towards the next waypoint
            transform.position = Vector3.MoveTowards(transform.position, path[currentNodeInPath].toNode.transform.position, speed * Time.deltaTime);

            // If "close enough" to count
            if (Vector3.Distance(transform.position, path[currentNodeInPath].toNode.transform.position) < 0.1f)
            {
                // Advance to next waypoint
                currentNodeInPath++;
            }
        }
        // else we reached the end
        else if (!reachedGoal)
        {
            Debug.Log("Reached Goal");
            GameManager.instance.virtualPet.ToggleOptions();
            SceneManager.UnloadSceneAsync("Fetch");
            GameManager.instance.virtualPet.GetComponent<Renderer>().enabled = true;
            GameManager.instance.virtualPet.EndTurn();
            reachedGoal = true;
        }
    }

    private NodeRecord FindInList(List<NodeRecord> targetList, Node testNode)
    {
        // It will help to have a helper node that can return a node that is in
        //       a list -- look at BreadthFirst's helper functions for details

        foreach (NodeRecord record in targetList)
        {
            if (record.node == testNode)
            {
                return record;
            }
        }
        return null;
    }

    private NodeRecord SmallestElement(List<NodeRecord> targetList)
    {
        // It will help to have a helper node that can return the
        //       node with the smallest value in a list!

        NodeRecord smallestElement = null;
        float smallestG = 0;
        float smallestH = 0;
        float smallestF = 0;

        foreach (NodeRecord nodeRecord in targetList)
        {
            float g = 0;
            float h = 0;
            float f = 0;
            //Debug.Log("f: " + f);
            NodeRecord currentNode = nodeRecord;

            // Work back through the path, accumulating connections
            while (currentNode.node != startNode)
            {
                //Add node cost
                g += currentNode.connection.cost;
                //(NOTE: Move to the previous connection)
                currentNode = FindInList(closedList, currentNode.connection.fromNode);
            }

            h = Vector3.Distance(nodeRecord.node.transform.position, targetNode.transform.position);
            f = h + g;
            if (smallestElement != null)
            {
                //Debug.Log("Current Smallest f: " + smallestF +
                //          "\nNext Node Cost: " + f);
            }
            if (smallestElement == null)
            {
                //Debug.Log("No current smallest");
                smallestElement = nodeRecord;
                smallestG = g;
                smallestH = h;
                smallestF = f;
            }
            else if (f <= smallestF)
            {
                if (f < smallestF)
                {
                    //Debug.Log("Next node is smaller");
                    smallestElement = nodeRecord;
                    smallestG = g;
                    smallestH = h;
                    smallestF = f;
                }
                else if (h <= smallestH)
                {
                    if (h < smallestH)
                    {
                        //Debug.Log("Next node is smaller");
                        smallestElement = nodeRecord;
                        smallestG = g;
                        smallestH = h;
                        smallestF = f;
                    }
                    else if (g < smallestG)
                    {
                        //Debug.Log("Next node is smaller");
                        smallestElement = nodeRecord;
                        smallestG = g;
                        smallestH = h;
                        smallestF = f;
                    }
                    //else
                    //{
                    //    Debug.Log("Smallest node is smaller");
                    //}
                }
                //else
                //{
                //    Debug.Log("Smallest node is smaller");
                //}
            }
            //else
            //{
            //    Debug.Log("Smallest node is smaller");
            //}
        }
        //Debug.Log("Final Cost" + smallestF);
        return smallestElement;
    }

    private bool ListContains(List<NodeRecord> testList, Node testNode)
    {
        // It may help to create a Helper Function to see if a list contains a node
        foreach (NodeRecord record in testList)
        {
            if (record.node == testNode)
            {
                return true;
            }
        }
        return false;
    }

    private void PlayFetch()
    {
        GameManager.instance.ConnectAllNodes();
        GameManager.instance.nodesParent.GetComponent<Fetch>().ShowGoal();
        GameManager.instance.nodesParent.GetComponent<Fetch>().ai.StartCoroutine("StartPathfinding");
        GameManager.instance.virtualPet.isPlaying = false;
    }
}
