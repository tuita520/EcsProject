using System.Collections.Generic;
using System;
using RDLog;

namespace ConfigData
{
    /// <summary>
    /// 单个数据
    /// </summary>
    public class Data
    {
		private DataList _dataList;

		private readonly Dictionary<string, Attribute> _attributes;
        /// <summary>
        /// 唯一ID
        /// </summary>
		public int Id { get; private set; }
        /// <summary>
        /// 唯一NAME
        /// </summary>
		public string Name { get; private set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; private set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public Data()
		{
			_attributes = new Dictionary<string, Attribute>();             
		}

		internal void SetOwner(DataList owner)
		{
			_dataList = owner;
		}

		internal void SetId(int id)
		{
			Id = id;
		}

		internal void SetName(string name)
		{
			Name = name;
		}

        internal void SetType(string type)
        {
           Type = type;
        }

		internal void SetAttribute(Attribute value)
		{
			if(_attributes.ContainsKey(value.Key))
			{
				Log.Error($"config '{_dataList.Key}' - data '{Id}' has more than one attribute named '{value.Key}.'");
				return;
			}
			_attributes.Add(value.Key, value);
		}
        /// <summary>
        /// 取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private Attribute Get(string key)
		{
			_attributes.TryGetValue(key, out var ret);
			return ret;
		}
        /// <summary>
        /// 取INT值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
		public int GetInt(string key)
		{
            var prob = Get(key);
            return prob?.GetInt() ?? 0;
		}

        public bool GetBoolean(string key)
        {
            var prob = Get(key);
            if (prob == null)
            {
                return false;
            }
            if(prob.GetString() == "true")
            {
                return true;
            }
            
	        if (prob.GetString() == "false")
            {
                return false;
            }
            
	        return prob.GetInt() > 0;
        }
        /// <summary>
        /// 取float值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
		public float GetFloat(string key)
        {
	        var prob = Get(key);
	        return prob?.GetFloat() ?? 0f;
        }
        /// <summary>
        /// 取string值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
		public string GetString(string key)
        {
	        var prob = Get(key);
	        return prob == null ? "" : prob.GetString();
        }

		/// <summary>
        ///  IEnumerable<T> implementation
		/// </summary>
		/// <returns></returns>
		public IEnumerator<KeyValuePair<string, Attribute>> GetEnumerator()
		{
			return _attributes.GetEnumerator();
		}
        /// <summary>
        /// Foreach
        /// </summary>
        /// <param name="callback"></param>
		public void Foreach(Action<Attribute> callback)
		{
			foreach(var pair in _attributes)
			{
				callback(pair.Value);
			}
		}
    }
}