using System;
using System.Collections.Generic;
using BigSeed.Math;

namespace Astar
{
    public interface IPathFindingMapInfo
    {
        int SizeX { get; }
        int SizeY { get; }
        List<int> GetWalkableNeighbours(int cellIndex);
        int GetCostBetweenNodes(int nodeIndex1, int nodeIndex2);
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

    public class DefaultMapInfo : IPathFindingMapInfo
    {
        readonly bool[] isWalkable;
        readonly int sizeX;
        readonly int sizeY;
        readonly int size;

        public DefaultMapInfo(int sizeX, int sizeY)
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

        public int SizeX
        {
            get { return sizeX; }
        }
        public int SizeY
        {
            get { return sizeY;  }
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

        public int GetCostBetweenNodes(int nodeIndex1, int nodeIndex2)
        {
            Vector2Int positionA = MapUtils.IndexToCoords(nodeIndex1, sizeX);
            Vector2Int positionB = MapUtils.IndexToCoords(nodeIndex2, sizeX);
            int dstX = Math.Abs(positionA.X - positionB.X);
            int dstY = Math.Abs(positionA.Y - positionB.Y);

            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);
            return 14 * dstX + 10 * (dstY - dstX);
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
        readonly int mapSize;
        readonly IPathFindingMapInfo mapInfo;
        readonly List<int> openSet;
        readonly HashSet<int> closedSet;

        public PathFinding(IPathFindingMapInfo mapInfo)
        {
            this.mapInfo = mapInfo;
            mapSizeX = mapInfo.SizeX;
            mapSize = mapInfo.SizeX * mapInfo.SizeY;

            openSet = new List<int>();
            closedSet = new HashSet<int>();
            nodes = new Node[mapSize];
        }

        int CoordsToIndex(int x, int y)
        {
            return MapUtils.CoordsToIndex(x, y, mapSizeX);
        }

        Vector2Int IndexToCoords(int index)
        {
            return new Vector2Int(index % mapSizeX, index / mapSizeX);
        }

        void Clear()
        {
            openSet.Clear();
            closedSet.Clear();

            for (int i = 0; i < mapSize; i++)
            {
                nodes[i] = new Node(i);
            }
        }

        public List<int> FindPath(int startX, int startY, int targetX, int targetY)
        {
            int startNode = CoordsToIndex(startX, startY);
            int targetNode = CoordsToIndex(targetX, targetY);

            Clear();

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

                foreach (int neighbourIndex in mapInfo.GetWalkableNeighbours(nodeIndex))
                {
                    Node neighbour = nodes[neighbourIndex];

                    if (closedSet.Contains(neighbourIndex))
                    {
                        continue;
                    }

                    int newCostToNeighbour = node.GCost + mapInfo.GetCostBetweenNodes(nodeIndex, neighbourIndex);
                    if (newCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbourIndex))
                    {
                        neighbour.GCost = newCostToNeighbour;
                        neighbour.HCost = mapInfo.GetCostBetweenNodes(neighbourIndex, targetNode);
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
    }
}
