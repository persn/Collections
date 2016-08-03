namespace LateStartStudio.Collections.Generic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a BinaryMinHeap implementation of a IPriorityQueue, so that the lowest ranking
    /// element is always first in the queue.
    /// </summary>
    /// <typeparam name="T">Specifies the type of element in the BinaryMinHeap.</typeparam>
    public class BinaryMinHeap<T> : IPriorityQueue<T> where T : IComparable<T>
    {
        private T[] heap;
        private object syncRoot;

        /// <summary>
        /// Initializes a new instance of the BinaryMinHeap class that is empty and has 0 capacity.
        /// </summary>
        public BinaryMinHeap()
        {
            this.heap = new T[0];
            this.Count = 0;
        }

        /// <summary>
        /// Initializes a new instance of the BinaryMinHeap class that is empty and has a specified
        /// initial capacity.
        /// </summary>
        /// <param name="capacity">The number of elements that the BinaryMinHeap can
        /// contain.</param>
        public BinaryMinHeap(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(
                    "capacity",
                    capacity,
                    "Variable capacity cannot be less than 0");
            }

            this.heap = new T[capacity];
            this.Count = 0;
        }

        /// <inheritdoc />
        public object SyncRoot
        {
            get
            {
                if (this.syncRoot == null)
                {
                    System.Threading.Interlocked.CompareExchange<object>(
                        ref this.syncRoot,
                        new object(),
                        null);
                }

                return this.syncRoot;
            }
        }

        /// <inheritdoc />
        public bool IsSynchronized
        {
            get { return false; }
        }

        /// <inheritdoc />
        public int Count { get; private set; }

        /// <inheritdoc />
        public void Enqueue(T item)
        {
            if (++this.Count > this.heap.Length)
            {
                this.ExpandQueue();
            }

            this.heap[this.Count - 1] = item;
            this.ShiftUp(this.Count - 1);
        }

        /// <inheritdoc />
        public T Peek()
        {
            return this.Count <= 0 ? default(T) : this.heap[0];
        }

        /// <inheritdoc />
        public T Dequeue()
        {
            if (this.Count <= 0)
            {
                return default(T);
            }

            T item = this.heap[0];

            this.heap[0] = this.heap[--this.Count];
            this.heap[this.Count] = default(T);
            this.ShiftDown(0);

            return item;
        }

        /// <inheritdoc />
        public int IndexOf(T item)
        {
            // One way to speed this up would be to search by traversing the heap, however this
            // would still be a worst case O(n) operation
            return Array.IndexOf(this.heap, item);
        }

        /// <inheritdoc />
        public bool Replace(int index, T item)
        {
            if (index < 0 || item.CompareTo(this.heap[index]) > -1)
            {
                return false;
            }

            this.heap[index] = item;
            this.ShiftUp(index);

            return true;
        }

        /// <inheritdoc />
        public bool Replace(T item)
        {
            return this.Replace(Array.IndexOf(this.heap, item), item);
        }

        /// <inheritdoc />
        public bool Contains(T item)
        {
            foreach (T heapElement in this.heap)
            {
                if (heapElement.Equals(item))
                {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc />
        public void CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            if (array.Rank != 1)
            {
                throw new ArgumentException("MultiDimensional array not supported", "array");
            }

            if (index < 0 || index > this.Count)
            {
                throw new ArgumentOutOfRangeException(
                    "index",
                    index,
                    "Argument must be in the range of 0 and the highest count of elements");
            }

            Array.Copy(this.heap, 0, array, 0, index);
        }

        /// <inheritdoc />
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <inheritdoc />
        public IEnumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        private static int GetParentIndex(int index)
        {
            return (index - 1) >> 1;
        }

        private static int GetLeftChildIndex(int index)
        {
            return (index << 1) + 1;
        }

        private static int GetRightChildIndex(int index)
        {
            return (index + 1) << 1;
        }

        private void ShiftUp(int index)
        {
            while (true)
            {
                int parentIndex = GetParentIndex(index);

                if (
                    index < 0 ||
                    parentIndex < 0 ||
                    this.heap[index].CompareTo(this.heap[parentIndex]) > 0)
                {
                    return;
                }

                this.Swap(parentIndex, index);
                index = parentIndex;
            }
        }

        private void ShiftDown(int index)
        {
            int newIndex;
            int leftIndex = GetLeftChildIndex(index);
            int rightIndex = GetRightChildIndex(index);

            if (rightIndex >= this.Count)
            {
                if (leftIndex >= this.Count)
                {
                    return;
                }

                newIndex = leftIndex;
            }
            else
            {
                if (this.heap[leftIndex].CompareTo(this.heap[rightIndex]) == -1)
                {
                    newIndex = leftIndex;
                }
                else
                {
                    newIndex = rightIndex;
                }
            }

            if (this.heap[index].CompareTo(this.heap[newIndex]) != 1)
            {
                return;
            }

            this.Swap(index, newIndex);
            this.ShiftDown(newIndex);
        }

        private T GetElement(int i)
        {
            return this.heap[i];
        }

        private void ExpandQueue()
        {
            T[] newQueue = this.heap.Length == 0 ?
                new T[1] : new T[this.heap.Length + (this.heap.Length << 1)];

            for (int i = 0; i < this.heap.Length; i++)
            {
                newQueue[i] = this.heap[i];
            }

            this.heap = newQueue;
        }

        private void Swap(int index1, int index2)
        {
            T temp = this.heap[index2];
            this.heap[index2] = this.heap[index1];
            this.heap[index1] = temp;
        }

        private struct Enumerator : IEnumerator<T>
        {
            private readonly BinaryMinHeap<T> priorityQueue;
            private int index;
            private T currentElement;

            internal Enumerator(BinaryMinHeap<T> priorityQueue)
            {
                this.priorityQueue = priorityQueue;
                this.index = -1;
                this.currentElement = default(T);
            }

            public T Current
            {
                get
                {
                    if (this.index < 0)
                    {
                        if (this.index == -1)
                        {
                            throw new InvalidOperationException("Enumerator not started");
                        }

                        throw new InvalidOperationException("Enumerator ended");
                    }

                    return this.currentElement;
                }
            }

            object IEnumerator.Current
            {
                get { return this.Current; }
            }

            public void Dispose()
            {
                this.index = -2;
                this.currentElement = default(T);
            }

            public bool MoveNext()
            {
                if (this.index == -2)
                {
                    return false;
                }

                this.index++;

                if (this.index == this.priorityQueue.Count)
                {
                    this.index = -2;
                    this.currentElement = default(T);
                    return false;
                }

                this.currentElement = this.priorityQueue.GetElement(this.index);
                return true;
            }

            public void Reset()
            {
                this.index = -1;
                this.currentElement = default(T);
            }
        }
    }
}
