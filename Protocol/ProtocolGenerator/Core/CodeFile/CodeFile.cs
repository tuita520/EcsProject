using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using RDLog;

namespace ProtocolGenerator.Core
{
    public class CodeFile:ProtoBuff
    {
        public readonly Dictionary<string, string> IdNameDic = new Dictionary<string, string>();
        
        public bool AddDataStructure(string structName, string msgId = null)
        {
            if (StructNameList.Contains(structName))
            {
                Log.Error($"file {Name} got an wrong struct name : {structName} ");
                return false;
            }

            if (!string.IsNullOrEmpty(msgId))
            {
                if (IdNameDic.TryGetValue(msgId, out var name))
                {
                    Log.Error($"file {Name} got an wrong msg id : {msgId}");
                    return false;
                }

                IdNameDic.Add(msgId, structName);
            }

            StructNameList.Add(structName);
            return true;
        }
        

    }
}