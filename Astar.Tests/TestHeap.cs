using System;
using System.Reflection;
using Tapas;

namespace Astar.Tests
{
//    public class HeapItems : IHeapItems<int>
//    {
//        private struct HeapNode
//        {
//            public int HeapIndex;
//            public int Value;
//
//            public HeapNode(int heapIndex, int value)
//            {
//                HeapIndex = heapIndex;
//                Value = value;
//            }
//        }
//
//        HeapNode[] items;
//
//        public HeapItems()
//        {
//            items = new HeapNode[16];
//
//            for (int i = 0; i < 16; i++)
//            {
//                items[15 - i] = new HeapNode(0, i);
//            }
//        }
//
//        public int Compare(int item1, int item2)
//        {
//            var node1 = items[item1];
//            var node2 = items[item2];
//
//            return node1.Value.CompareTo(node2.Value);
//        }
//
//        public int GetItemHeapIndex(int item)
//        {
//            return items[item].HeapIndex;
//        }
//
//        public void SetItemHeapIndex(int item, int newIndex)
//        {
//            items[item].HeapIndex = newIndex;
//        }
//
//        public void SetItem(int index, int value)
//        {
//            var item = items[index];
//            item.Value = value;
//            items[index] = item;
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
    
    public class TestHeap
    {
        [Test]
        public void Add_in_order_with_integers()
        {
            var heap = new Heap<int>(16);

            heap.Add(0);
            heap.Add(1);
            heap.Add(2);
            heap.Add(3);
            heap.Add(4);
            heap.Add(5);

            var items = (int[])typeof(Heap<int>).GetField("items", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(heap);

            Assert.Equal(5, items[0]);
            Assert.Equal(3, items[1]);
            Assert.Equal(4, items[2]);
            Assert.Equal(0, items[3]);
            Assert.Equal(2, items[4]);
            Assert.Equal(1, items[5]);
        }
        
        [Test]
        public void Add_in_order_with_heap_items()
        {
            var heap = new Heap<HeapItem>(16);

            heap.Add(new HeapItem(0, 0, 0));
            heap.Add(new HeapItem(1, 1, 0));
            heap.Add(new HeapItem(2, 2, 0));
            heap.Add(new HeapItem(3, 3, 0));
            heap.Add(new HeapItem(4, 4, 0));
            heap.Add(new HeapItem(5, 5, 0));

            var items = (HeapItem[])typeof(Heap<HeapItem>).GetField("items", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(heap);

            Assert.Equal(0, items[0].NodeIndex);
            Assert.Equal(1, items[1].NodeIndex);
            Assert.Equal(2, items[2].NodeIndex);
            Assert.Equal(3, items[3].NodeIndex);
            Assert.Equal(4, items[4].NodeIndex);
            Assert.Equal(5, items[5].NodeIndex);
        }

        [Test]
        public void Add_with_always_a_sorting_to_the_top()
        {
            var heap = new Heap<HeapItem>(16);

            heap.Add(new HeapItem(5, 5, 0));
            heap.Add(new HeapItem(4, 4, 0));
            heap.Add(new HeapItem(3, 3, 0));
            heap.Add(new HeapItem(2, 2, 0));
            heap.Add(new HeapItem(1, 1, 0));
            heap.Add(new HeapItem(0, 0, 0));

            var items = (HeapItem[])typeof(Heap<HeapItem>).GetField("items", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(heap);

            Assert.Equal(0, items[0].NodeIndex);
            Assert.Equal(2, items[1].NodeIndex);
            Assert.Equal(1, items[2].NodeIndex);
            Assert.Equal(5, items[3].NodeIndex);
            Assert.Equal(3, items[4].NodeIndex);
            Assert.Equal(4, items[5].NodeIndex);
        }

//        [Test]
//        public void Test_contains()
//        {
//            var heap = new Heap<HeapItem>(16);
//
//            var item0 = new HeapItem(0, 0, 0);
//            var item5 = new HeapItem(5, 5, 0);
//            var item6 = new HeapItem(6, 6, 0);
//
//            heap.Add(item0);
//            heap.Add(new HeapItem(1, 1, 0));
//            heap.Add(new HeapItem(2, 2, 0));
//            heap.Add(new HeapItem(3, 3, 0));
//            heap.Add(new HeapItem(4, 4, 0));
//            heap.Add(item5);
//
//            Assert.Equal(true, heap.Contains(item0));
//            Assert.Equal(true, heap.Contains(item5));
//            Assert.Equal(false, heap.Contains(item6));
//        }

        [Test]
        public void Test_remove_first_with_integers()
        {
            var heap = new Heap<int>(16);

            heap.Add(5);
            heap.Add(4);
            heap.Add(3);
            heap.Add(2);
            heap.Add(1);
            heap.Add(0);

            Assert.Equal(5, heap.RemoveFirst());
            Assert.Equal(4, heap.RemoveFirst());
            Assert.Equal(3, heap.RemoveFirst());
            Assert.Equal(2, heap.RemoveFirst());
            Assert.Equal(1, heap.RemoveFirst());
            Assert.Equal(0, heap.RemoveFirst());
        }
        
        [Test]
        public void Test_remove_first_with_heap_items()
        {
            var heap = new Heap<HeapItem>(16);

            heap.Add(new HeapItem(5, 5, 0));
            heap.Add(new HeapItem(4, 4, 0));
            heap.Add(new HeapItem(3, 3, 0));
            heap.Add(new HeapItem(2, 2, 0));
            heap.Add(new HeapItem(1, 1, 0));
            heap.Add(new HeapItem(0, 0, 0));

            Assert.Equal(0, heap.RemoveFirst().NodeIndex);
            Assert.Equal(1, heap.RemoveFirst().NodeIndex);
            Assert.Equal(2, heap.RemoveFirst().NodeIndex);
            Assert.Equal(3, heap.RemoveFirst().NodeIndex);
            Assert.Equal(4, heap.RemoveFirst().NodeIndex);
            Assert.Equal(5, heap.RemoveFirst().NodeIndex);
        }

//        [Test]
//        public void Test_update_item()
//        {
//            var heap = new Heap<int>(16);
//
//            heap.Add(5);
//            heap.Add(4);
//            heap.Add(3);
//            heap.Add(2);
//            heap.Add(1);
//            heap.Add(0);
//
////            heapItems.SetItem(1, 100);
//            heap.UpdateItem(1);
//
//            var items = (int[])typeof(Heap<int>).GetField("items", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(heap);
//
//            Assert.Equal(1, items[0]);
//            Assert.Equal(2, items[1]);
//            Assert.Equal(0, items[2]);
//            Assert.Equal(5, items[3]);
//            Assert.Equal(3, items[4]);
//            Assert.Equal(4, items[5]);
//        }
    }
}
