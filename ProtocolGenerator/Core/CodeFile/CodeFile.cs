using System;
using System.Collections.Generic;
using System.Text;
using RDLog;

namespace ProtocolGenerator.Core
{
    public class CodeFile
    {
        public string FullName { get; set; }
        public string Name { get; set; }
        
        public string Syntax { get; set; }

        public string Package { get; set; }
        
        public string Option { get; set; }

        
        public List<string> ImportList = new List<string>();

        public readonly Dictionary<string, string> IdNameDic = new Dictionary<string, string>();
        
        public readonly List<string> NameList = new List<string>();
        
        public StringBuilder ProtoBufText = new StringBuilder();
        
        public bool AddDataStructure(string structName, string msgId = null)
        {
            if (NameList.Contains(structName))
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
            
            NameList.Add(structName);
            return true;
        }
        
        public bool SetPackage(string package)
        {
            Package = package;
            return true;
        }

        public bool SetSyntax(string syntax)
        {
            Syntax = syntax;
            return true;
        }

        public bool SetOption(string option)
        {
            Option = option;
            return true;
        }

        public bool AddImport(string importName)
        {
            if (ImportList.Contains(importName))
            {
                return false;
            }
            ImportList.Add(importName);
            return true;
        }
        
        public void AddProtoBuffData(StringBuilder protoLine)
        {
            ProtoBufText.Append(protoLine);
        }
    }
}