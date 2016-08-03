namespace LateStartStudio.Collections.Tests.Generic
{
    using System;
    using System.Collections;
    using Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class BinaryMinHeapTests : PriorityQueueTests
    {
        [Test]
        public void CapacityConstructor()
        {
            BinaryMinHeap<int> heap = new BinaryMinHeap<int>(5);

            Assert.IsNotNull(heap);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CapacityConstructorThrowArgumentOutOfRangeException()
        {
            BinaryMinHeap<int> heap = new BinaryMinHeap<int>(-1);
        }

        [Test]
        public void CopyToSimple()
        {
            this.PriorityQueue.Enqueue(1);
            this.PriorityQueue.Enqueue(2);
            this.PriorityQueue.Enqueue(3);

            int[] copyTo = new int[this.PriorityQueue.Count];
            this.PriorityQueue.CopyTo(copyTo, this.PriorityQueue.Count);

            Assert.AreEqual(new[] { 1, 2, 3 }, copyTo);
        }

        [Test]
        public void GetEnumeratorSimple()
        {
            this.PriorityQueue.Enqueue(1);
            this.PriorityQueue.Enqueue(2);
            this.PriorityQueue.Enqueue(3);

            IEnumerator enumerator = this.PriorityQueue.GetEnumerator();

            enumerator.MoveNext();
            Assert.AreEqual(1, enumerator.Current);
            enumerator.MoveNext();
            Assert.AreEqual(2, enumerator.Current);
            enumerator.MoveNext();
            Assert.AreEqual(3, enumerator.Current);
        }

        protected override IPriorityQueue<int> CreateInstance()
        {
            return new BinaryMinHeap<int>();
        }
    }
}
