using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadsLessons
{
    internal class MultithreadList<T> where T : class
    {
        private object _lock { get; set; }
        public List<T> List { get; private set;}

        public MultithreadList()
        {
            List = new List<T>();
            _lock = new object();
        }

        public void MultithreadAdd(T item)
        {
            lock(_lock)
            {
                List.Add(item);
            }
        }

        public void MiltithreadRemove(T item)
        {
            lock (_lock)
            {
                List.Remove(item);
            }
        }


    }
}
