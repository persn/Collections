namespace LateStartStudio.Collections.Generic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a queue of elements ordered so that the lowest or highest ranking element
    /// is always the first in the queue.
    /// </summary>
    /// <typeparam name="T">Specifies the type of element in the IPriorityQueue.</typeparam>
    public interface IPriorityQueue<T> : IEnumerable<T>, ICollection where T : IComparable<T>
    {
        /// <summary>
        /// Adds an object to the IPriorityQueue.
        /// </summary>
        /// <param name="item">The object to add to the IPriorityQueue.</param>
        void Enqueue(T item);

        /// <summary>
        /// Returns the object that has the lowest or highest ranking in the IPriorityQueue
        /// without removing it.
        /// </summary>
        /// <returns>The object with the lowest or highest ranking in the IPriorityQueue.</returns>
        T Peek();

        /// <summary>
        /// Returns the object that has the lowest or highest ranking in the IPriorityQueue
        /// and removes it.
        /// </summary>
        /// <returns>The object with the lowest or highest ranking in the IPriorityQueue.</returns>
        T Dequeue();

        /// <summary>
        /// Return the first index in the IPriorityQueue that matches with the item provided as an
        /// argument.
        /// </summary>
        /// <param name="item">The item to search for in the IPriorityQueue.</param>
        /// <returns>The first index that matches with the item provided as an argument.</returns>
        int IndexOf(T item);

        /// <summary>
        /// Replaces the item in the IPriorityQueue with the item provided as an argument that
        /// matches the provided index.
        /// </summary>
        /// <param name="index">The index position to replace item.</param>
        /// <param name="item">The item to insert into the IPriorityQueue.</param>
        /// <returns>Returns a boolean indicating if the replacement was successful.</returns>
        bool Replace(int index, T item);

        /// <summary>
        /// Replaces the first found item in the IPriorityQueue with the item provided as an
        /// argument.
        /// </summary>
        /// <param name="item">The item to insert into the IPriorityQueue.</param>
        /// <returns>Returns a boolean indicating if the replacement was successful.</returns>
        bool Replace(T item);

        /// <summary>
        /// Searches the IPriorityQueue for the item provided as an argument.
        /// </summary>
        /// <param name="item">The item to search for inside the IPriorityQueue.</param>
        /// <returns>Returns a boolean indicating if the item was found.</returns>
        bool Contains(T item);
    }
}
