using System;
using System.Collections.Generic;

namespace Astar
{
    public struct Vector2Int
    {
        public readonly int X;
        public readonly int Y;

        public Vector2Int(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public static class MapUtils
    {
        public static int CoordsToIndex(int x, int y, int sizeX)
        {
            return y * sizeX + x;
        }

        public static Vector2Int IndexToCoords(int index, int sizeX)
        {
            return new Vector2Int(index % sizeX, index / sizeX);
        }
    }
    
    public class PathMapLayer
    {
        readonly bool[] isWalkable;
        readonly int sizeX;
        readonly int sizeY;
        readonly int size;

        public PathMapLayer(int sizeX, int sizeY)
        {
            this.sizeX = sizeX;
            this.sizeY = sizeY;

            size = sizeX * sizeY;

            isWalkable = new bool[size];

            for (int i = 0; i < size; i++)
            {
                isWalkable[i] = true;
            }
        }

        public void SetWalkable(int x, int y, bool walkable)
        {
            isWalkable[MapUtils.CoordsToIndex(x, y, sizeX)] = walkable;
        }

        public bool IsWalkable(int x, int y)
        {
            return isWalkable[MapUtils.CoordsToIndex(x, y, sizeX)];
        }

        public List<int> GetWalkableNeighbours(int cellIndex)
        {
            List<int> neighbours = new List<int>(8);
            Vector2Int cellCoords = MapUtils.IndexToCoords(cellIndex, sizeX);

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                        continue;

                    int checkX = cellCoords.X + i;
                    int checkY = cellCoords.Y + j;

                    if (checkX >= 0 && checkX < sizeX && checkY >= 0 && checkY < sizeY)
                    {
                        int neighbourIndex = MapUtils.CoordsToIndex(checkX, checkY, sizeX);

                        if (isWalkable[neighbourIndex])
                        {
                            neighbours.Add(neighbourIndex);
                        }
                    }
                }
            }

            return neighbours;
        }
    }

    public struct Node
    {
        public readonly int Index;
        public int ParentIndex;

        public int GCost;
        public int HCost;

        public Node(int index)
        {
            this.Index = index;
            ParentIndex = 0;

            GCost = 0;
            HCost = 0;
        }

        public int FCost => GCost + HCost;
    }

    public class PathFinding
    {
        readonly Node[] nodes;
        readonly int mapSizeX;

        public PathFinding(int sizeX, int sizeY)
        {
            mapSizeX = sizeX;
            int mapSize = sizeX * sizeY;
            nodes = new Node[mapSize];

            for (int i = 0; i < mapSize; i++)
            {
                nodes[i] = new Node(i);
            }
        }

        int CoordsToIndex(int x, int y)
        {
            return MapUtils.CoordsToIndex(x, y, mapSizeX);
        }

        Vector2Int IndexToCoords(int index)
        {
            return new Vector2Int(index % mapSizeX, index / mapSizeX);
        }
        
        public List<int> FindPath(int startX, int startY, int targetX, int targetY, PathMapLayer pathMapLayer)
        {
            int startNode = CoordsToIndex(startX, startY);
            int targetNode = CoordsToIndex(targetX, targetY);

            // TODO: Just clear.
            List<int> openSet = new List<int>();
            HashSet<int> closedSet = new HashSet<int>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                // Get the cheapest open node.
                int nodeIndex = openSet[0];
                Node node = nodes[nodeIndex];
                for (int i = 1; i < openSet.Count; i++)
                {
                    Node currentOpenNode = nodes[openSet[i]];
                    if (currentOpenNode.FCost < node.FCost || currentOpenNode.FCost == node.FCost)
                    {
                        if (currentOpenNode.HCost < node.HCost)
                        {
                            nodeIndex = currentOpenNode.Index;
                            node = nodes[nodeIndex];
                        }
                    }
                }

                openSet.Remove(nodeIndex);
                closedSet.Add(nodeIndex);

                if (nodeIndex == targetNode)
                {
                    return RetracePath(startNode, targetNode);
                }

                // TODO: Does a call to iterator called multiple time in foreach.
                var neighbours = pathMapLayer.GetWalkableNeighbours(nodeIndex);
                foreach (int neighbourIndex in neighbours)
                {
                    Node neighbour = nodes[neighbourIndex];
                    
                    if (closedSet.Contains(neighbourIndex))
                    {
                        continue;
                    }

                    int newCostToNeighbour = node.GCost + GetCostBetweenNodes(nodeIndex, neighbourIndex);
                    if (newCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbourIndex))
                    {
                        neighbour.GCost = newCostToNeighbour;
                        neighbour.HCost = GetCostBetweenNodes(neighbourIndex, targetNode);
                        neighbour.ParentIndex = nodeIndex;
                        nodes[neighbourIndex] = neighbour;

                        if (!openSet.Contains(neighbourIndex))
                            openSet.Add(neighbourIndex);
                    }
                }
            }

            return null;
        }

        List<int> RetracePath(int startNode, int endNode)
        {
            List<int> path = new List<int>();
            int currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = nodes[currentNode].ParentIndex;
            }
            path.Reverse();

            return path;

        }

        // TODO: This should end up in PathMapLayer.
        int GetCostBetweenNodes(int nodeA, int nodeB)
        {
            Vector2Int positionA = IndexToCoords(nodeA);
            Vector2Int positionB = IndexToCoords(nodeB);
            int dstX = Math.Abs(positionA.X - positionB.X);
            int dstY = Math.Abs(positionA.Y - positionB.Y);

            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);
            return 14 * dstX + 10 * (dstY - dstX);
        }
    }
}
