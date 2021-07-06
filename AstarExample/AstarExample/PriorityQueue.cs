using System;
using System.Collections.Generic;
using System.Text;

namespace AstarExample
{
    class PriorityQueue<T> where T : IComparable<T>
    {
        private List<T> _heap = new List<T>();

        public int Count { get { return _heap.Count; } }

        public T Pop()
        {
            T retVal = _heap[0];

            int lastIndex = _heap.Count - 1;
            _heap[0] = _heap[lastIndex];
            _heap.RemoveAt(lastIndex);

            int now = 0;
            while (now < lastIndex)
            {
                int left = now * 2 + 1;
                int right = (now + 1) * 2;

                int next = now;
                if (left < lastIndex && _heap[next].CompareTo(_heap[left]) < 0)
                {
                    next = left;
                }

                if (right < lastIndex && _heap[next].CompareTo(_heap[right]) < 0)
                {
                    next = right;
                }

                if (now == next)
                {
                    break;
                }

                T temp = _heap[next];
                _heap[next] = _heap[now];
                _heap[now] = temp;

                now = next;
            }

            return retVal;
        }

        public void Push(T data)
        {
            _heap.Add(data);

            int now = _heap.Count - 1;
            while (now != 0)
            {
                int next = (now - 1) / 2;

                if (_heap[next].CompareTo(_heap[now]) >= 0)
                {
                    break;
                }

                T temp = _heap[next];
                _heap[next] = _heap[now];
                _heap[now] = temp;

                now = next;
            }
        }

        public void Clear() => _heap.Clear();
    }
}
