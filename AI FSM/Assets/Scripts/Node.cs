using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<NodeConnection> connections;
    public bool isBlocked = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {

    }

    public bool ContainsLinkTo(Node target)
    {
        foreach (NodeConnection connection in connections)
        {
            if (connection.toNode == target)
            {
                return true;
            }
        }
        return false;
    }

    public void AddUniqueLinkTo(Node target)
    {
        if (!ContainsLinkTo(target))
        {
            NodeConnection temp = new NodeConnection();
            temp.fromNode = this;
            temp.toNode = target;
            temp.cost = Vector3.Distance(transform.position, target.transform.position);
            connections.Add(temp);
        }
    }
}

[System.Serializable]
public class NodeConnection
{
    public Node fromNode;
    public Node toNode;
    public float cost;
}
