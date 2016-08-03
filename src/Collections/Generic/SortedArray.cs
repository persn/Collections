namespace LateStartStudio.Collections.Generic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a SortedArray implementation of a IPriorityQueue, so that the lowest ranking
    /// element is always first in the queue.
    /// DISCLAIMER:
    /// This class is a wrapper around List with forced sort. It is not performance oriented and
    /// exists only for the purpose of developing new features of IPriorityQueue quickly and setup
    /// unit-tests that other implementations of IPriorityQueue must pass. It is not recommended
    /// for production use.
    /// </summary>
    /// <typeparam name="T">Specifies the type of element in the SortedArray.</typeparam>
    public class SortedArray<T> : IPriorityQueue<T> where T : IComparable<T>
    {
        private readonly List<T> sortedArray = new List<T>();
        private object syncRoot;

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
        public int Count
        {
            get { return this.sortedArray.Count; }
        }

        /// <inheritdoc />
        public void Enqueue(T item)
        {
            this.sortedArray.Add(item);
            this.sortedArray.Sort((a, b) => b.CompareTo(a));
        }

        /// <inheritdoc />
        public T Peek()
        {
            return this.Count <= 0 ? default(T) : this.sortedArray[this.Count - 1];
        }

        /// <inheritdoc />
        public T Dequeue()
        {
            if (this.Count == 0)
            {
                return default(T);
            }

            T item = this.sortedArray[this.Count - 1];
            this.sortedArray.Remove(item);
            return item;
        }

        /// <inheritdoc />
        public int IndexOf(T item)
        {
            return this.sortedArray.IndexOf(item);
        }

        /// <inheritdoc />
        public bool Replace(int index, T item)
        {
            if (index < 0 || index >= this.Count || item.CompareTo(this.sortedArray[index]) > -1)
            {
                return false;
            }

            this.sortedArray[index] = item;
            return true;
        }

        /// <inheritdoc />
        public bool Replace(T item)
        {
            T oldItem = this.sortedArray.Find(i => i.Equals(item));

            if (oldItem == null || item.CompareTo(oldItem) > -1)
            {
                return false;
            }

            this.sortedArray.Remove(oldItem);
            this.sortedArray.Add(item);
            this.sortedArray.Sort((a, b) => b.CompareTo(a));

            return true;
        }

        /// <inheritdoc />
        public bool Contains(T item)
        {
            return this.sortedArray.Contains(item);
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

            Array.Copy(this.sortedArray.ToArray(), 0, array, 0, index);
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

        private T GetElement(int index)
        {
            return this.sortedArray[index];
        }

        private struct Enumerator : IEnumerator<T>
        {
            private readonly SortedArray<T> priorityQueue;
            private int index;
            private T currentElement;

            internal Enumerator(SortedArray<T> priorityQueue)
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
