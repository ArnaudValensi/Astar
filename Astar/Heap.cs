// MIT License
// Copyright (c) 2017 Sebastian Lague
// https://www.youtube.com/watch?v=3Dw5d7PlcTM

namespace Astar
{
    public class Heap<T> {
        T[] items;
        int currentItemCount;
       // private IComparer<T> comparer;
       IHeapItems<T> heapItems;
	
        public Heap(int maxHeapSize, IHeapItems<T> heapItems)
        {
            this.heapItems = heapItems;
            items = new T[maxHeapSize];
        }

        public void Clear()
        {
            currentItemCount = 0;
        }
	
        public void Add(T item) {
            heapItems.SetItemHeapIndex(item, currentItemCount);
            //item.HeapIndex = currentItemCount;
            items[currentItemCount] = item;
            SortUp(item);
            currentItemCount++;
        }

        public T RemoveFirst() {
            T firstItem = items[0];
            currentItemCount--;
            items[0] = items[currentItemCount];
            heapItems.SetItemHeapIndex(items[0], 0);
            //items[0].HeapIndex = 0;
            SortDown(items[0]);
            return firstItem;
        }

        public void UpdateItem(T item) {
            SortUp(item);
            
            // NOTE: If we want to use the heap with items that can have priority reduced, we have to
            // add `SortDown(item)`.
        }

        public int Count {
            get {
                return currentItemCount;
            }
        }

        public bool Contains(T item)
        {
            int itemIndex = heapItems.GetItemHeapIndex(item);
            return Equals(items[itemIndex], item);

            //return heapItems.Compare(items[itemIndex], item) == 0;
            //return Equals(items[item.HeapIndex], item);
        }

        void SortDown(T item) {
            while (true) {
                int itemIndex = heapItems.GetItemHeapIndex(item);
                int childIndexLeft = itemIndex * 2 + 1;
                int childIndexRight = itemIndex * 2 + 2;
                int swapIndex = 0;

                if (childIndexLeft < currentItemCount) {
                    swapIndex = childIndexLeft;

                    if (childIndexRight < currentItemCount) {
                        // if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0) {
                        if (heapItems.Compare(items[childIndexLeft], items[childIndexRight]) < 0) {
                            swapIndex = childIndexRight;
                        }
                    }

                    // if (item.CompareTo(items[swapIndex]) < 0) {
                    if (heapItems.Compare(item, items[swapIndex]) < 0) {
                        Swap (item,items[swapIndex]);
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
	
        void SortUp(T item)
        {
            int itemIndex = heapItems.GetItemHeapIndex(item);
            int parentIndex = (itemIndex-1)/2;
		
            while (true) {
                T parentItem = items[parentIndex];
                // if (item.CompareTo(parentItem) > 0) {
                if (heapItems.Compare(item, parentItem) > 0) {
                    Swap (item,parentItem);
                }
                else {
                    break;
                }

                parentIndex = (itemIndex-1)/2;
            }
        }
	
        void Swap(T itemA, T itemB)
        {
            int itemIndexA = heapItems.GetItemHeapIndex(itemA);
            int itemIndexB = heapItems.GetItemHeapIndex(itemB);
            
            items[itemIndexA] = itemB;
            items[itemIndexB] = itemA;
            
            heapItems.SetItemHeapIndex(itemA, itemIndexB);
            heapItems.SetItemHeapIndex(itemB, itemIndexA);
        }
    }

    public interface IHeapItems<T> {
        int Compare(T item1, T item2);
        int GetItemHeapIndex(T item);
        void SetItemHeapIndex(T item, int newIndex);
    }
}