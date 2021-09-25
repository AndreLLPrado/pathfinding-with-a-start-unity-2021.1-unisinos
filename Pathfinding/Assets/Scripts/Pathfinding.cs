using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Transform seeker, target;
    Grid grid;

    void Awake()
    {
        grid = GetComponent<Grid>();
    }

     void Update()
    {
        findPath(seeker.position, target.position);
    }
    void findPath(Vector3 startPosition, Vector3 targetPosition)
    {
        Node startNode = grid.NodeFromWorldPoint(startPosition);
        Node targetNode = grid.NodeFromWorldPoint(targetPosition);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for(int i = 0; i < openSet.Count; i++)
            {
                if(openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if(currentNode == targetNode)
            {
                retracePath(startNode, targetNode);
                return;
            }

            foreach(Node neighBour in grid.getNeighBours(currentNode))
            {
                if(!neighBour.walkable || closedSet.Contains(neighBour))
                {
                    continue;
                }

                int newMovimentCostToNeighBour = currentNode.gCost + GetDistance(currentNode, neighBour);
                if(newMovimentCostToNeighBour < neighBour.gCost || !openSet.Contains(neighBour))
                {
                    neighBour.gCost = newMovimentCostToNeighBour;
                    neighBour.hCost = GetDistance(neighBour, targetNode);
                    neighBour.parent = currentNode;

                    if (!openSet.Contains(neighBour))
                    {
                        openSet.Add(neighBour);
                    }
                }
            }
        }

        void retracePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while(currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }
            path.Reverse();

            grid.path = path;
        }

        int GetDistance(Node A, Node B)
        {
            int disX = Mathf.Abs(A.gridX - B.gridX);
            int disY = Mathf.Abs(A.gridY - B.gridY);

            if(disX > disY)
            {
                return 14 * disY * 10 * (disX - disY);
            }
            return 14 * disX * 10 * (disY - disX);
        }
    }
}
