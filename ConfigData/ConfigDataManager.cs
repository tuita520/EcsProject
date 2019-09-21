using System;
using System.Collections.Generic;
using RDLog;
using Utility;

namespace ConfigData
{
    /// <summary>
    /// 整合xml
    /// </summary>
    public class ConfigDataManager : Singleton<ConfigDataManager>
    {
        private AConfigParser _configParser;

        private Dictionary<string, DataList> _dataListDic;

        public Dictionary<string, Dictionary<string,DataList>> _typeDataListDic;

        /// <summary>
        /// 初始化List
        /// </summary>
        public void Init(AConfigParser configParser)
        {
            _dataListDic = new Dictionary<string, DataList>();
            _typeDataListDic = new Dictionary<string, Dictionary<string,DataList>>();
            _configParser = configParser;
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public bool Parse(string filename, string text = null)
        {
            var dataList = _configParser.Parse(filename, text);
            if (dataList == null)
            {
                Log.Debug($"${filename} is null");
                return false;
            }

            if (_dataListDic.ContainsKey(dataList.Key))
            {
                Log.Error($" {filename} id:{dataList.Key} exist ");
                return true;
            }

            _dataListDic.Add(dataList.Key, dataList);
            return true;
        }

        /// <summary>
        /// 获取LIST
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public DataList GetDataList(string key)
        {
            _dataListDic.TryGetValue(key, out var dataList);
            return dataList;
        }
        
        /// <summary>
        /// 获取LIST
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public DataList GetDataList(string key,string dataType)
        {
            _typeDataListDic.TryGetValue(key, out var idList);
            if (idList == null)
            {
                return null;
            }
        
            idList.TryGetValue(dataType, out var dataList);
            return dataList;
        }

        /// <summary>
        /// 获取DATA
        /// </summary>
        /// <param name="idspaceID"></param>
        /// <param name="classID"></param>
        /// <returns></returns>
        public Data GetData(string key, int dataId)
        {
            Data data = null;

            var dataList = GetDataList(key);
            if (dataList != null)
            {
                data = dataList.Get(dataId);
            }

            return data;
        }

        /// <summary>
        /// 获取DATA
        /// </summary>
        /// <param name="idspaceID"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public Data GetData(string key, string dataName)
        {
            Data data = null;

            var dataList = GetDataList(key);
            if (dataList != null)
            {
                data = dataList.Get(dataName);
            }

            return data;
        }

        /// <summary>
        /// 获取类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public ValueType GetValueType(string value, out object output)
        {
            return _configParser.ParseValue(value, out output);
        }
   
    }
}