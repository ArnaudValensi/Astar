using System;
using System.Collections.Generic;
using BigSeed.Math;
using BigSeed.Utils;

namespace Astar
{
    public interface IPathFindingMapInfo
    {
        int SizeX { get; }
        int SizeY { get; }
        List<int> GetWalkableNeighbours(int cellIndex);
        int GetCostBetweenNodes(int nodeIndex1, int nodeIndex2);
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
            List<int> neighbours = new List<int>(8); // TODO: Pass this in parameter.
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

//    public class HeapItems : IHeapItems<int>
//    {
//        private Node[] nodes;
//        public HeapItems(Node[] nodes)
//        {
//            this.nodes = nodes;
//        }
//        
//        public int Compare(int item1, int item2)
//        {
//            Node node1 = nodes[item1];
//            Node node2 = nodes[item2];
//            
//            int compare = node1.FCost.CompareTo(node2.FCost);
//
//            if (compare == 0)
//            {
//                compare = node1.HCost.CompareTo(node2.HCost);
//            }
//
//            return -compare;
//        }
//
//        public int GetItemHeapIndex(int item)
//        {
//            return nodes[item].HeapIndex;
//        }
//
//        public void SetItemHeapIndex(int item, int newIndex)
//        {
//            nodes[item].HeapIndex = newIndex;
//        }
//    }

    public struct HeapItem : IComparable<HeapItem>
    {
        public int NodeIndex;
        public int FCost;
        public int HCost;

        public HeapItem(int nodeIndex, int fCost, int hCost)
        {
            NodeIndex = nodeIndex;
            FCost = fCost;
            HCost = hCost;
        }
        
        public int CompareTo(HeapItem other)
        {
            int compare = FCost.CompareTo(other.FCost);

            if (compare == 0)
            {
                compare = HCost.CompareTo(other.HCost);
            }

            return -compare;
        }
    }

    public struct Node
    {
        public readonly int Index;
        public int ParentIndex;

        public int GCost;
        public int HCost;

        public int HeapIndex;

        public Node(int index)
        {
            this.Index = index;
            ParentIndex = 0;

            GCost = 0;
            HCost = 0;

            HeapIndex = 0;
        }

        public int FCost => GCost + HCost;

        public HeapItem ToHeapItem()
        {
            return new HeapItem(Index, FCost, HCost);
        }
    }

    public class PathFinding
    {
        readonly Node[] nodes;
        readonly int mapSizeX;
        readonly int mapSize;
        readonly IPathFindingMapInfo mapInfo;
        readonly Heap<HeapItem> openSet;
        readonly HashSet<int> closedSet;

        public PathFinding(IPathFindingMapInfo mapInfo)
        {
            this.mapInfo = mapInfo;
            mapSizeX = mapInfo.SizeX;
            mapSize = mapInfo.SizeX * mapInfo.SizeY;

            nodes = new Node[mapSize];
            openSet = new Heap<HeapItem>(mapSize);
            closedSet = new HashSet<int>();
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
            Clear();

            int startNode = CoordsToIndex(startX, startY);
            int targetNode = CoordsToIndex(targetX, targetY);
            
            openSet.Add(nodes[startNode].ToHeapItem());

            while (openSet.Count > 0)
            {
                // Get the cheapest open node.
//                int nodeIndex = openSet[0];
                int nodeIndex = openSet.RemoveFirst().NodeIndex;
                Node node = nodes[nodeIndex];

                closedSet.Add(nodeIndex);

                if (nodeIndex == targetNode)
                {
                    return RetracePath(startNode, targetNode);
                }

                var neighbours = mapInfo.GetWalkableNeighbours(nodeIndex);
                foreach (int neighbourIndex in neighbours)
                {
                    Node neighbour = nodes[neighbourIndex];

                    if (closedSet.Contains(neighbourIndex))
                    {
                        continue;
                    }

                    int newCostToNeighbour = node.GCost + mapInfo.GetCostBetweenNodes(nodeIndex, neighbourIndex);
                    if (newCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour.ToHeapItem()))
                    {
                        neighbour.GCost = newCostToNeighbour;
                        neighbour.HCost = mapInfo.GetCostBetweenNodes(neighbourIndex, targetNode);
                        neighbour.ParentIndex = nodeIndex;
                        nodes[neighbourIndex] = neighbour;

                        if (!openSet.Contains(neighbour.ToHeapItem()))
                        {
                            openSet.Add(neighbour.ToHeapItem());
                        }
                        else 
                        {
                            openSet.UpdateItem(neighbour.ToHeapItem());
                        }
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
