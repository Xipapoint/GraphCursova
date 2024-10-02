using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphCursova.Structures
{
    public class PriorityQueueHeap
    {
        private List<Tuple<int, double>> heap;

        public PriorityQueueHeap()
        {
            heap = new List<Tuple<int, double>>();
        }

        public void Enqueue(int element, double priority)
        {
            heap.Add(new Tuple<int, double>(element, priority));
            BubbleUp(heap.Count - 1);
        }

        private void BubbleUp(int index)
        {
            int currentIndex = index;
            while (currentIndex > 0)
            {
                int parentIndex = (currentIndex - 1) / 2;
                if (heap[currentIndex].Item2 >= heap[parentIndex].Item2) break;
                Swap(currentIndex, parentIndex);
                currentIndex = parentIndex;
            }
        }

        private void Swap(int i, int j)
        {
            var temp = heap[i];
            heap[i] = heap[j];
            heap[j] = temp;
        }

        public Tuple<int, double> Dequeue()
        {
            if (heap.Count == 0)
            {
                Console.WriteLine("Heap is empty");
                return null;
            }
            var min = heap[0];
            heap[0] = heap[heap.Count - 1];
            heap.RemoveAt(heap.Count - 1);
            BubbleDown(0);
            return min;
        }

        public void BubbleDown(int pos)
        {
            List<Tuple<int, double>> heap = this.heap;
            int currentIndex = pos;

            while (true)
            {
                int left = 2 * currentIndex + 1;
                int right = 2 * currentIndex + 2;
                int smallestIndex = currentIndex;

                if (left < heap.Count && heap[left].Item2 < heap[smallestIndex].Item2)
                {
                    smallestIndex = left;
                }

                if (right < heap.Count && heap[right].Item2 < heap[smallestIndex].Item2)
                {
                    smallestIndex = right;
                }

                if (smallestIndex != currentIndex)
                {
                    Tuple<int, double>  temp = heap[currentIndex];
                    heap[currentIndex] = heap[smallestIndex];
                    heap[smallestIndex] = temp;
                    currentIndex = smallestIndex;
                }
                else
                {
                    break;
                }
            }
        }


        public bool IsEmpty()
        {
            return heap.Count == 0;
        }
    }
}
