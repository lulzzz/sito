using System.Collections.Generic;
using System.Linq;

namespace Maddalena.Core.Numl.AI.Collections
{
    /// <summary>
    /// A priority queue.
    /// </summary>
    /// <typeparam name="P">Key type.</typeparam>
    /// <typeparam name="V">Value type.</typeparam>
    public class PriorityQueue<P, V>
    {
        private readonly SortedDictionary<P, Queue<V>> list = new SortedDictionary<P, Queue<V>>();

        /// <summary>
        /// Enqueues the specified priority.
        /// </summary>
        /// <param name="priority">The priority.</param>
        /// <param name="value">The value.</param>
        public void Enqueue(P priority, V value)
        {
            if (!list.TryGetValue(priority, out var q))
            {
                q = new Queue<V>();
                list.Add(priority, q);
            }
            q.Enqueue(value);
            Count++;
        }

        /// <summary>
        /// Dequeues this instance.
        /// </summary>
        /// <returns>V.</returns>
        public V Dequeue()
        {
            // will throw if there isn�t any first element!
            var pair = list.First();
            var v = pair.Value.Dequeue();
            if (pair.Value.Count == 0) // nothing left of the top priority.
                list.Remove(pair.Key);
            Count--;
            return v;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is empty.
        /// </summary>
        /// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
        public bool IsEmpty
        {
            get { return !list.Any(); }
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count { get; private set; } = 0;
    }
}
