namespace LateStartStudio.Collections.Tests.Generic
{
    using System;
    using System.Collections;
    using Collections.Generic;
    using NUnit.Framework;

    public abstract class PriorityQueueTests
    {
        protected IPriorityQueue<int> PriorityQueue { get; private set; }

        [SetUp]
        public void Init()
        {
            this.PriorityQueue = this.CreateInstance();
        }

        [Test]
        public void Contains()
        {
            this.PriorityQueue.Enqueue(1);

            Assert.IsTrue(this.PriorityQueue.Contains(1));
            Assert.IsFalse(this.PriorityQueue.Contains(2));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CopyToThrowArgumentNullException()
        {
            this.PriorityQueue.CopyTo(null, index: 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CopyToThrowArgumentExceptionWhenMultiDimArray()
        {
            int[,] copyTo = new int[1, 1];

            this.PriorityQueue.CopyTo(copyTo, 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CopyToThrowArgumentOutOfRangeException()
        {
            int[] copyTo = new int[1];

            this.PriorityQueue.CopyTo(copyTo, -1);
        }

        [Test]
        public void Count()
        {
            const int Expected = 5;

            for (int i = 0; i < Expected; i++)
            {
                this.PriorityQueue.Enqueue(i);
            }

            Assert.AreEqual(Expected, this.PriorityQueue.Count);
        }

        [Test]
        public void PeekItem()
        {
            this.PriorityQueue.Enqueue(5);

            Assert.AreEqual(this.PriorityQueue.Peek(), 5);
        }

        [Test]
        public void DequeueItemWhenEmpty()
        {
            Assert.AreEqual(0, this.PriorityQueue.Dequeue());
        }

        [Test]
        public void EnqueuePopItemWhenHardcodedArbitraryOrder()
        {
            const int Node0 = 1;
            const int Node1 = 2;
            const int Node2 = 3;
            const int Node3 = 4;
            const int Node4 = 5;
            const int Node5 = 6;
            const int Node6 = 7;
            const int Node7 = 8;
            const int Node8 = 9;
            const int Node9 = 10;

            this.PriorityQueue.Enqueue(Node5);
            this.PriorityQueue.Enqueue(Node3);
            this.PriorityQueue.Enqueue(Node6);
            this.PriorityQueue.Enqueue(Node7);
            this.PriorityQueue.Enqueue(Node1);
            this.PriorityQueue.Enqueue(Node9);
            this.PriorityQueue.Enqueue(Node0);
            this.PriorityQueue.Enqueue(Node2);
            this.PriorityQueue.Enqueue(Node8);
            this.PriorityQueue.Enqueue(Node4);

            Assert.AreEqual(10, this.PriorityQueue.Count);
            Assert.AreEqual(Node0, this.PriorityQueue.Dequeue());
            Assert.AreEqual(9, this.PriorityQueue.Count);
            Assert.AreEqual(Node1, this.PriorityQueue.Dequeue());
            Assert.AreEqual(8, this.PriorityQueue.Count);
            Assert.AreEqual(Node2, this.PriorityQueue.Dequeue());
            Assert.AreEqual(7, this.PriorityQueue.Count);
            Assert.AreEqual(Node3, this.PriorityQueue.Dequeue());
            Assert.AreEqual(6, this.PriorityQueue.Count);
            Assert.AreEqual(Node4, this.PriorityQueue.Dequeue());
            Assert.AreEqual(5, this.PriorityQueue.Count);
            Assert.AreEqual(Node5, this.PriorityQueue.Dequeue());
            Assert.AreEqual(4, this.PriorityQueue.Count);
            Assert.AreEqual(Node6, this.PriorityQueue.Dequeue());
            Assert.AreEqual(3, this.PriorityQueue.Count);
            Assert.AreEqual(Node7, this.PriorityQueue.Dequeue());
            Assert.AreEqual(2, this.PriorityQueue.Count);
            Assert.AreEqual(Node8, this.PriorityQueue.Dequeue());
            Assert.AreEqual(1, this.PriorityQueue.Count);
            Assert.AreEqual(Node9, this.PriorityQueue.Dequeue());
            Assert.AreEqual(0, this.PriorityQueue.Count);
        }

        [Test]
        public void GetEnumeratorReachEndAndReset()
        {
            this.PriorityQueue.Enqueue(1);

            IEnumerator enumerator = this.PriorityQueue.GetEnumerator();

            enumerator.MoveNext();
            Assert.AreEqual(1, enumerator.Current);

            Assert.IsFalse(enumerator.MoveNext()); // False on end of enumerator
            Assert.IsFalse(enumerator.MoveNext()); // False if already reached end

            // Test reset to start
            enumerator.Reset();
            enumerator.MoveNext();
            Assert.AreEqual(1, enumerator.Current);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetEnumeratorCurrentThrowInvalidOperationNotStarted()
        {
            IEnumerator enumerator = this.PriorityQueue.GetEnumerator();

            int test = (int)enumerator.Current;
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetEnumeratorCurrentThrowInvalidOperationEnded()
        {
            this.PriorityQueue.Enqueue(1);

            IEnumerator enumerator = this.PriorityQueue.GetEnumerator();
            enumerator.MoveNext();
            enumerator.MoveNext();

            int test = (int)enumerator.Current;
        }

        [Test]
        public void ReplaceWhenEmptySet()
        {
            Assert.IsFalse(this.PriorityQueue.Replace(1));
        }

        [Test]
        public void ReplaceWhenSingleItem()
        {
            this.PriorityQueue.Enqueue(2);

            Assert.IsTrue(this.PriorityQueue.Replace(0, 1));
            Assert.AreEqual(1, this.PriorityQueue.Count);
            Assert.AreEqual(1, this.PriorityQueue.Dequeue());
        }

        [Test]
        public void ReplaceWhenMultipleItems()
        {
            this.PriorityQueue.Enqueue(2);
            this.PriorityQueue.Enqueue(3);
            this.PriorityQueue.Enqueue(4);
            this.PriorityQueue.Enqueue(5);
            this.PriorityQueue.Enqueue(6);

            Assert.IsTrue(this.PriorityQueue.Replace(4, 1));
            Assert.AreEqual(1, this.PriorityQueue.Peek());
        }

        [Test]
        public void ReplaceWhenItemNotInSet()
        {
            this.PriorityQueue.Enqueue(1);

            Assert.IsFalse(this.PriorityQueue.Replace(2));
        }

        [Test]
        public void IsSynchronsizedFalse()
        {
            Assert.IsFalse(this.PriorityQueue.IsSynchronized);
        }

        [Test]
        public void SyncRootNotNull()
        {
            Assert.IsNotNull(this.PriorityQueue.SyncRoot);
        }

        [Test]
        public void IndexOfGenericTest()
        {
            this.PriorityQueue.Enqueue(5);
            this.PriorityQueue.Enqueue(1);
            this.PriorityQueue.Enqueue(10);

            Assert.GreaterOrEqual(this.PriorityQueue.IndexOf(5), 0);
            Assert.GreaterOrEqual(this.PriorityQueue.IndexOf(1), 0);
            Assert.GreaterOrEqual(this.PriorityQueue.IndexOf(10), 0);

            Assert.AreEqual(-1, this.PriorityQueue.IndexOf(2));
        }

        protected abstract IPriorityQueue<int> CreateInstance();
    }
}
