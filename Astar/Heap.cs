// MIT License
// Copyright (c) 2017 Sebastian Lague
// https://www.youtube.com/watch?v=3Dw5d7PlcTM
// https://youtu.be/t0Cq6tVNRBA

using System;

namespace Astar
{
    public class Heap<T> where T : IComparable<T>
    {
        T[] items;
        int currentItemCount;
        // private IComparer<T> comparer;
	
        public Heap(int maxHeapSize)
        {
            items = new T[maxHeapSize];
        }

        public void Clear()
        {
            currentItemCount = 0;
        }
	
        public void Add(T item) {
            //item.HeapIndex = currentItemCount;
            items[currentItemCount] = item;
            SortUp(currentItemCount);
            currentItemCount++;
        }

        public T RemoveFirst() {
            T firstItem = items[0];
            currentItemCount--; // items[currentItemCount] = 0 ?
            items[0] = items[currentItemCount];
            //items[0].HeapIndex = 0;
            SortDown(0);
            return firstItem;
        }

        public void UpdateItem(T item) {
            //SortUp(item);
            
            // NOTE: If we want to use the heap with items that can have priority reduced, we have to
            // add `SortDown(item)`.
            // We can add a boolean passed to the constructor to disable the sort down.
        }

        public int Count => currentItemCount;

        public bool Contains(T item)
        {
            return true;
            //return Equals(items[itemIndex], item);

            //return heapItems.Compare(items[itemIndex], item) == 0;
            //return Equals(items[item.HeapIndex], item);
        }

        void SortDown(int itemIndex) {
            while (true) {
                // int itemIndex = heapItems.GetItemHeapIndex(item);
                int childIndexLeft = itemIndex * 2 + 1;
                int childIndexRight = childIndexLeft + 1;
                int swapIndex = 0;

                if (childIndexLeft < currentItemCount) {
                    swapIndex = childIndexLeft;

                    if (childIndexRight < currentItemCount) {
                        if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0) {
                            swapIndex = childIndexRight;
                        }
                    }

                    if (items[itemIndex].CompareTo(items[swapIndex]) < 0) {
                        Swap (itemIndex,swapIndex);
                    }
                    else {
                        return;
                    }

                }
                else {
                    return;
                }
            }
        }
	
        void SortUp(int itemIndex)
        {
            // int itemIndex = heapItems.GetItemHeapIndex(item);
            var item = items[itemIndex];
            int parentIndex = (itemIndex-1)/2;
		
            while (true) {
                T parentItem = items[parentIndex];
                if (item.CompareTo(parentItem) > 0) {
                    Swap (itemIndex,parentIndex);
                    itemIndex = parentIndex;
                }
                else {
                    break;
                }

                // itemIndex = heapItems.GetItemHeapIndex(item);
                parentIndex = (itemIndex-1)/2;
            }
        }
	
        void Swap(int itemIndexA, int itemIndexB)
        {
            var itemA = items[itemIndexA];
            items[itemIndexA] = items[itemIndexB];
            items[itemIndexB] = itemA;
        }
    }
}