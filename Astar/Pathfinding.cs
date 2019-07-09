using System;
using System.Collections.Generic;

namespace Astar
{
    public struct Vector2Int
    {
        public int x;
        public int y;

        public Vector2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
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
        bool[] isWalkable;
        int sizeX;
        int sizeY;
        int size;

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

                    int checkX = cellCoords.x + i;
                    int checkY = cellCoords.y + j;

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
        public int index;

        public int gCost;
        public int hCost;
        public int parentIndex;

        public Node(int index)
        {
            this.index = index;
            
            gCost = 0;
            hCost = 0;
            parentIndex = 0;
        }

        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }
    }

    public class Pathfinding
    {
        Node[] nodes;
        int mapSizeX;
        int mapSize;

        public Pathfinding(int sizeX, int sizeY)
        {
            mapSizeX = sizeX;
            mapSize = sizeX * sizeY;
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
        
        public Vector2Int IndexToCoords(int index)
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
                    if (currentOpenNode.fCost < node.fCost || currentOpenNode.fCost == node.fCost)
                    {
                        if (currentOpenNode.hCost < node.hCost)
                        {
                            nodeIndex = currentOpenNode.index;
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

                    int newCostToNeighbour = node.gCost + GetCostBetweenNodes(nodeIndex, neighbourIndex);
                    if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbourIndex))
                    {
                        neighbour.gCost = newCostToNeighbour;
                        neighbour.hCost = GetCostBetweenNodes(neighbourIndex, targetNode);
                        neighbour.parentIndex = nodeIndex;
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
                currentNode = nodes[currentNode].parentIndex;
            }
            path.Reverse();

            return path;

        }

        // TODO: This should end up in PathMapLayer.
        int GetCostBetweenNodes(int nodeA, int nodeB)
        {
            Vector2Int positionA = IndexToCoords(nodeA);
            Vector2Int positionB = IndexToCoords(nodeB);
            int dstX = Math.Abs(positionA.x - positionB.x);
            int dstY = Math.Abs(positionA.y - positionB.y);

            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);
            return 14 * dstX + 10 * (dstY - dstX);
        }
    }
}
