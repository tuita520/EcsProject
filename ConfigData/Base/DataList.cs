using System;
using System.Collections.Generic;
using System.Linq;
using RDLog;

namespace ConfigData
{
    /// <summary>
    /// 多组数据
    /// </summary>
    public class DataList : IEnumerable<KeyValuePair<int, Data>>
    {
        /// <summary>
        /// 回调
        /// </summary>
        /// <param name="data"></param>
        public delegate void OnDataCallback(Data data);

        /// <summary>
        /// XML名
        /// </summary>
        private string key;
        public string Key => key;

        private int Count => dataListByID.Count;

        private readonly Dictionary<int, Data> dataListByID;

        private readonly Dictionary<string, Data> dataListByName;
        private readonly Dictionary<string, List<Data>> dataListByType;

        internal DataList()
        {
            dataListByID = new Dictionary<int, Data>();
            dataListByName = new Dictionary<string, Data>();
            dataListByType = new Dictionary<string, List<Data>>();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="key"></param>
        public void Init(string key)
        {
            this.key = key;
        }

        /// <summary>
        /// 添加新DATA
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool AddData(Data data)
        {
            if (dataListByID.ContainsKey(data.Id))
            {
                Log.Error($"config '{Key}' has duplicated date id '{data.Id}'");
                return false;
            }

            if (data.Name != null && dataListByName.ContainsKey(data.Name))
            {
                Log.Error($"config '{Key}' has duplicated date name '{data.Name}'");
                return false;
            }

            data.SetOwner(this);
            dataListByID.Add(data.Id, data);
            if (data.Name != null)
            {
                dataListByName.Add(data.Name, data);
            }

            if (data.Type != null)
            {
                if (!dataListByType.TryGetValue(data.Type,out var typeList))
                {
                    typeList = new List<Data>();
                    dataListByType.Add(data.Type, typeList);
                }
                typeList.Add(data);
            }

            return true;
        }

        public Data Get(int id)
        {
            dataListByID.TryGetValue(id, out var data);
            return data;
        }

        public Data Get(string name)
        {
            dataListByName.TryGetValue(name, out var data);
            return data;
        }

        public List<Data> GetByGroup(string type)
        {
            dataListByType.TryGetValue(type, out var dataList);
            return dataList;
        }


        public Data GetByIndex(int index)
        {
            if (index >= 0 && index < Count)
            {
                return dataListByID.Values.ElementAt(index);
            }

            return null;
        }

        public void ForeachData(OnDataCallback callback)
        {
            foreach (var pair in dataListByID)
            {
                callback(pair.Value);
            }
        }

        public IEnumerator<KeyValuePair<int, Data>> GetEnumerator()
        {
            return dataListByID.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return dataListByID.GetEnumerator();
        }
    }
}