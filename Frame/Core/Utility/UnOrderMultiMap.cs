using System.Collections.Generic;

namespace Frame.Core.Utility
{
    public class UnOrderMultiMap<T, K>
    {
        private readonly Dictionary<T, List<K>> dictionary = new Dictionary<T, List<K>>();

        // 重用list
        private readonly Queue<List<K>> queue = new Queue<List<K>>();

        public Dictionary<T, List<K>> GetDictionary()
        {
            return this.dictionary;
        }

        public void Add(T t, K k)
        {
            List<K> list;
            this.dictionary.TryGetValue(t, out list);
            if (list == null)
            {
                list = this.FetchList();
                this.dictionary[t] = list;
            }
            list.Add(k);
        }

        public bool Remove(T t)
        {
            List<K> list = null;
            this.dictionary.TryGetValue(t, out list);
            if (list != null)
            {
                this.RecycleList(list);
            }
            return this.dictionary.Remove(t);
        }


        /// <summary>
        /// 返回内部的list
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public List<K> this[T t]
        {
            get
            {
                List<K> list;
                this.dictionary.TryGetValue(t, out list);
                return list;
            }
        }
        
        public bool ContainsKey(T t)
        {
            return this.dictionary.ContainsKey(t);
        }
        
        //取
        private List<K> FetchList()
        {
            if (this.queue.Count <= 0) return new List<K>();
            var list = this.queue.Dequeue();
            list.Clear();
            return list;
        }
        
        //回收
        private void RecycleList(List<K> list)
        {
            // 防止暴涨
            if (this.queue.Count > 100)
            {
                return;
            }
            list.Clear();
            this.queue.Enqueue(list);
        }

    }
}