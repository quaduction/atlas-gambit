using System.Collections.Generic;

namespace GRisk.Util
{
    public class FixedSizedQueue<T> : Queue<T>
    {
        public int Limit { get; set; }

        public new void Enqueue(T obj)
        {
            base.Enqueue(obj);
            
            while (Count > Limit)
            {
                Dequeue();
            }
        }
    }

}