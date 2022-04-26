using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{

    public Vector3 position;
    public float G = 0.0f;
    public float H = 0.0f;
    public float F = 0.0f;

    public bool isClosed = false;
    public Node Parent = null;

    public GameObject zone = null;

    public void ResetNode()
    {
        G = H = F = 0.0f;
        isClosed = false;
        Parent = null;
    }

}

public class GridSystem : MonoBehaviour
{

    public Vector2Int gridSize;

    [SerializeField] private GameObject gridPrefab;
    private List<List<Node>> grid = new List<List<Node>>();

    //This function will prepare on game launched.
    private void Awake()
    {

        for(int x = 0; x < gridSize.x; x++)
        {
            List<Node> column = new List<Node>();
            for(int y = 0; y < gridSize.y; y++)
            {
                Node nodeObj = new Node();

                Vector3 nodePos = new Vector3((float)x + 0.5f, 0.0f, (float)y + 0.5f);

                //The node object will have the real worldspace position for collision checking.
                nodeObj.position = nodeObj.position + nodePos;

                GameObject newGrid = Instantiate(gridPrefab, transform);
                newGrid.transform.localPosition = nodePos;
                newGrid.name = "NodeX" + x.ToString() + "Y" + y.ToString();
                newGrid.SetActive(true);

                nodeObj.zone = newGrid;

                column.Add(nodeObj);
            }
            grid.Add(column);
        }
    }


    //This will get the node object at the position given, this should allow easier alignment.
    public Node getNodeAtPos(Vector3 position)
    {
        if (gridSize.x == 0 || gridSize.y == 0)
            return null; //The grid is empty... Nothing to return.

        position -= transform.position; //Make this position relative to the grid system.

        //Extract the floored X and Y component from the position's X and Z, since Y is up and down?
        int gridX = Mathf.FloorToInt(position.x);

        if (gridX < 0 || gridX >= gridSize.x)
            return null; //Out of grid range, automatically return nothing.

        int gridY = Mathf.FloorToInt(position.z);

        if (gridY < 0 || gridY >= gridSize.y)
            return null; //Out of grid range, automatically return nothing.

        //Okay a valid node is found, return the node in the grid.
        return grid[gridX][gridY];
    }


    //This will attempt to find a path and return if a possible one is found.
    public List<Vector3> findPath(Vector3 startPos, Vector3 endPos)
    {

        //First we need to find our start and end nodes.
        Node startNode = getNodeAtPos(startPos);
        Node endNode = getNodeAtPos(endPos);

        if (startNode == null || endNode == null)
            return null; //No valid start or end node, immediately return empty path.

        //Okay valid start and end node found, reset the grid in preparation.
        for(int x = gridSize.x - 1; x > -1; x--)
            for(int y = gridSize.y; y > -1; y--)
                grid[x][y].ResetNode();


        //Find the distance between start and end.
        float distance = (endNode.position - startNode.position).magnitude;

        //Setting the G, H and F cost of both nodes, G = distance from start, H = distance from end, F = G + H.
        startNode.G = endNode.H = 0.0f;
        startNode.H = endNode.G = startNode.F = endNode.F = distance;

        //Create a new open list and add the starting node to this list.
        List<Node> open = new List<Node>();
        open.Add(startNode);

        //Begin the A* pathfinding algorithm!
        while(open.Count > 0)
        {

            //First find the lowest F costing node.
            Node current = open[0];
            float minF = Mathf.Infinity;
            for (int i = open.Count - 1; i > 0; i--)
                if(current == null || open[i].F < current.F)
                    current = open[i];

            if (current == endNode)
                break; //If the current node found is the end node, then we found a path!

            //Okay, remove the current node from the open list and make it closed instead.
            open.Remove(current);
            current.isClosed = true;

            //Begin checking/searching for neighbours.

        }


        return null; //If it reaches here, then it means there is no valid path to be found!
    }


}
