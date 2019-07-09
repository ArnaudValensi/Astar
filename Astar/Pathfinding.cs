using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Astar
{
    public class Pathfinding
    {
        public List<Node> FindPath(Vector2 startPos, Vector2 targetPos)
        {
            //Node startNode = grid.NodeFromWorldPoint(startPos);
            //Node targetNode = grid.NodeFromWorldPoint(targetPos);

            //List<Node> openSet = new List<Node>();
            //HashSet<Node> closedSet = new HashSet<Node>();
            //openSet.Add(startNode);

            //while (openSet.Count > 0)
            //{
            //    Node node = openSet[0];
            //    for (int i = 1; i < openSet.Count; i++)
            //    {
            //        if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
            //        {
            //            if (openSet[i].hCost < node.hCost)
            //                node = openSet[i];
            //        }
            //    }

            //    openSet.Remove(node);
            //    closedSet.Add(node);

            //    if (node == targetNode)
            //    {
            //        return RetracePath(startNode, targetNode);
            //    }

            //    foreach (Node neighbour in grid.GetNeighbours(node))
            //    {
            //        if (!neighbour.walkable || closedSet.Contains(neighbour))
            //        {
            //            continue;
            //        }

            //        int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
            //        if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
            //        {
            //            neighbour.gCost = newCostToNeighbour;
            //            neighbour.hCost = GetDistance(neighbour, targetNode);
            //            neighbour.parent = node;

            //            if (!openSet.Contains(neighbour))
            //                openSet.Add(neighbour);
            //        }
            //    }
            //}

            return null;
        }

        List<Node> RetracePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }
            path.Reverse();

            return path;

        }

        int GetDistance(Node nodeA, Node nodeB)
        {
            int dstX = Math.Abs(nodeA.gridX - nodeB.gridX);
            int dstY = Math.Abs(nodeA.gridY - nodeB.gridY);

            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);
            return 14 * dstX + 10 * (dstY - dstX);
        }
    }

    public class Node
    {
        public bool walkable;
        public Vector3 worldPosition;
        public int gridX;
        public int gridY;

        public int gCost;
        public int hCost;
        public Node parent;

        public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
        {
            walkable = _walkable;
            worldPosition = _worldPos;
            gridX = _gridX;
            gridY = _gridY;
        }

        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }
    }

    //internal struct CostNode
    //{
    //    public int index;
    //    public int cost;

    //    public CostNode(int index, int cost)
    //    {
    //        this.index = index;
    //        this.cost = cost;
    //    }
    //}
}
