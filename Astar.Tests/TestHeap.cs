using System.Reflection;
using Tapas;

namespace Astar.Tests
{
    public class HeapItems : IHeapItems<int>
    {
        private struct HeapNode
        {
            public int HeapIndex;
            public int Value;

            public HeapNode(int heapIndex, int value)
            {
                HeapIndex = heapIndex;
                Value = value;
            }
        }

        HeapNode[] items;

        public HeapItems()
        {
            items = new HeapNode[16];

            for (int i = 0; i < 16; i++)
            {
                items[15 - i] = new HeapNode(0, i);
            }
        }

        public int Compare(int item1, int item2)
        {
            var node1 = items[item1];
            var node2 = items[item2];

            return node1.Value.CompareTo(node2.Value);
        }

        public int GetItemHeapIndex(int item)
        {
            return items[item].HeapIndex;
        }

        public void SetItemHeapIndex(int item, int newIndex)
        {
            items[item].HeapIndex = newIndex;
        }

        public void SetItem(int index, int value)
        {
            var item = items[index];
            item.Value = value;
            items[index] = item;
        }
    }
    public class TestHeap
    {
        [Test]
        public void Add_in_order()
        {
            var heapItems = new HeapItems();
            var heap = new Heap<int>(16, heapItems);

            heap.Add(0);
            heap.Add(1);
            heap.Add(2);
            heap.Add(3);
            heap.Add(4);
            heap.Add(5);

            var items = (int[])typeof(Heap<int>).GetField("items", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(heap);

            Assert.Equal(0, items[0]);
            Assert.Equal(1, items[1]);
            Assert.Equal(2, items[2]);
            Assert.Equal(3, items[3]);
            Assert.Equal(4, items[4]);
            Assert.Equal(5, items[5]);
        }

        [Test]
        public void Add_with_always_a_sorting_to_the_top()
        {
            var heapItems = new HeapItems();
            var heap = new Heap<int>(16, heapItems);

            heap.Add(5);
            heap.Add(4);
            heap.Add(3);
            heap.Add(2);
            heap.Add(1);
            heap.Add(0);

            var items = (int[])typeof(Heap<int>).GetField("items", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(heap);

            Assert.Equal(0, items[0]);
            Assert.Equal(2, items[1]);
            Assert.Equal(1, items[2]);
            Assert.Equal(5, items[3]);
            Assert.Equal(3, items[4]);
            Assert.Equal(4, items[5]);
        }

        [Test]
        public void Test_contains()
        {
            var heapItems = new HeapItems();
            var heap = new Heap<int>(16, heapItems);

            heap.Add(0);
            heap.Add(1);
            heap.Add(2);
            heap.Add(3);
            heap.Add(4);
            heap.Add(5);

            Assert.Equal(true, heap.Contains(0));
            Assert.Equal(true, heap.Contains(5));
            Assert.Equal(false, heap.Contains(6));
        }

        [Test]
        public void Test_remove_first()
        {
            var heapItems = new HeapItems();
            var heap = new Heap<int>(16, heapItems);

            heap.Add(5);
            heap.Add(4);
            heap.Add(3);
            heap.Add(2);
            heap.Add(1);
            heap.Add(0);

            Assert.Equal(0, heap.RemoveFirst());
            Assert.Equal(1, heap.RemoveFirst());
            Assert.Equal(2, heap.RemoveFirst());
            Assert.Equal(3, heap.RemoveFirst());
            Assert.Equal(4, heap.RemoveFirst());
            Assert.Equal(5, heap.RemoveFirst());
        }

        [Test]
        public void Test_update_item()
        {
            var heapItems = new HeapItems();
            var heap = new Heap<int>(16, heapItems);

            heap.Add(5);
            heap.Add(4);
            heap.Add(3);
            heap.Add(2);
            heap.Add(1);
            heap.Add(0);

            heapItems.SetItem(1, 100);
            heap.UpdateItem(1);

            var items = (int[])typeof(Heap<int>).GetField("items", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(heap);

            Assert.Equal(1, items[0]);
            Assert.Equal(2, items[1]);
            Assert.Equal(0, items[2]);
            Assert.Equal(5, items[3]);
            Assert.Equal(3, items[4]);
            Assert.Equal(4, items[5]);
        }
    }
}
