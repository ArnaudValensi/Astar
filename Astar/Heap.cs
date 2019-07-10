// MIT License
// Copyright (c) 2017 Sebastian Lague
// https://www.youtube.com/watch?v=3Dw5d7PlcTM
// https://youtu.be/t0Cq6tVNRBA

using System;

namespace Astar
{
    public class Heap<T> where T : IHeapItem<T>
    {
        T[] items;
        int currentItemCount;
	
        public Heap(int maxHeapSize)
        {
            items = new T[maxHeapSize];
        }

        public void Clear()
        {
            currentItemCount = 0;
        }
	
        public void Add(T item) {
            item.HeapIndex = currentItemCount;
            items[currentItemCount] = item;
            SortUp(item);
            currentItemCount++;
        }

        public T RemoveFirst() {
            T firstItem = items[0];
            currentItemCount--;
            items[0] = items[currentItemCount];
            items[0].HeapIndex = 0;
            SortDown(items[0]);
            return firstItem;
        }

        public void UpdateItem(T item) {
            SortUp(item);
            
            // NOTE: If we want to use the heap with items that can have priority reduced, we have to
            // add `SortDown(item)`.
            // We can add a boolean passed to the constructor to disable the sort down.
        }

        public int Count => currentItemCount;

        public bool Contains(T item)
        {
            return Equals(items[item.HeapIndex], item);
        }

        void SortDown(T item) {
            while (true) {
                int childIndexLeft = item.HeapIndex * 2 + 1;
                int childIndexRight = childIndexLeft + 1;
                int swapIndex = 0;

                if (childIndexLeft < currentItemCount) {
                    swapIndex = childIndexLeft;

                    if (childIndexRight < currentItemCount) {
                        if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0) {
                            swapIndex = childIndexRight;
                        }
                    }

                    if (item.CompareTo(items[swapIndex]) < 0) {
                        // We need to get the swapped item in the case T it a value type.
                        item = Swap (item,items[swapIndex]);
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
	
        void SortUp(T item) {
            int parentIndex = (item.HeapIndex-1)/2;
		
            while (true) {
                T parentItem = items[parentIndex];
                if (item.CompareTo(parentItem) > 0) {
                    // We need to get the swapped item in the case T it a value type.
                    item = Swap (item,parentItem);
                }
                else {
                    break;
                }

                parentIndex = (item.HeapIndex-1)/2;
            }
        }
	
        T Swap(T itemA, T itemB) {
            int itemAIndex = itemA.HeapIndex;
            itemA.HeapIndex = itemB.HeapIndex;
            itemB.HeapIndex = itemAIndex;
            
            items[itemB.HeapIndex] = itemB;
            items[itemA.HeapIndex] = itemA;

            return itemA;
        }
    }
    
    public interface IHeapItem<T> : IComparable<T> {
        int HeapIndex {
            get;
            set;
        }
    }
}