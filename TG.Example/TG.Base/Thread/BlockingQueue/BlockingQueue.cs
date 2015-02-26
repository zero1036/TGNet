using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TS.Base
{
    public class BlockingQueue<T>
    {
        private readonly Queue<T> queue = new Queue<T>();
        private readonly int maxSize;
        public BlockingQueue(int maxSize) { this.maxSize = maxSize; }

        /// <summary>
        /// 队列尾插入元素
        /// </summary>
        /// <param name="item"></param>
        public void Enqueue(T item)
        {
            lock (queue)
            {
                while (queue.Count >= maxSize)
                {
                    Monitor.Wait(queue);
                }
                queue.Enqueue(item);
                if (queue.Count == 1)
                {
                    // wake up any blocked dequeue
                    Monitor.PulseAll(queue);
                }
            }
        }
        /// <summary>
        /// 队列前段抽取元素并移出队列
        /// </summary>
        /// <returns></returns>
        public T Dequeue()
        {
            lock (queue)
            {
                while (queue.Count == 0)
                {
                    Monitor.Wait(queue);
                }
                T item = queue.Dequeue();
                if (queue.Count == maxSize - 1)
                {
                    // wake up any blocked enqueue
                    Monitor.PulseAll(queue);
                }
                return item;
            }
        }

        bool closing;
        public void Close()
        {
            lock (queue)
            {
                closing = true;
                Monitor.PulseAll(queue);
            }
        }
        public bool TryDequeue(out T value)
        {
            lock (queue)
            {
                while (queue.Count == 0)
                {
                    if (closing)
                    {
                        value = default(T);
                        return false;
                    }
                    Monitor.Wait(queue);
                }
                value = queue.Dequeue();
                if (queue.Count == maxSize - 1)
                {
                    // wake up any blocked enqueue
                    Monitor.PulseAll(queue);
                }
                return true;
            }
        }

        public int Count
        {
            get
            {
                return queue.Count;
            }
        }
    }
}
