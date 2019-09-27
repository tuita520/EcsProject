using System;
using System.Collections.Generic;
using System.Text;
using ProtocolGenerator.Core;
using RDLog;

namespace ProtocolBuild.Generator.Core
{
    public class CodeFile
    {
        public string FullName { get; set; }
        public string Name { get; set; }
        public string Syntax { get; set; }

        public PackageType PackageType = PackageType.Both;

        public readonly Dictionary<string, string> IdNameDic = new Dictionary<string, string>();
        public readonly Dictionary<string, string> NameIdDic = new Dictionary<string, string>();

        public StringBuilder ProtoBufData { get; set; }

        public bool AddMessageNameId(string msgName, string msgId = null)
        {
            if (NameIdDic.TryGetValue(msgName, out var id))
            {
                Log.Error($"file {Name} got an wrong msg name : {msgName} ");
                return false;
            }

            if (IdNameDic.TryGetValue(msgId, out var name))
            {
                Log.Error($"file {Name} got an wrong msg id : {msgId}");
                return false;
            }

            if (!string.IsNullOrEmpty(msgId))
            {
                IdNameDic.Add(msgId, msgName);
            }

            NameIdDic.Add(msgName, msgId);
            return true;
        }


        public bool SetPackageType(string package)
        {
            if ( !Enum.TryParse<PackageType>(package ,out var packageType))
            {
                return false;
            }
            PackageType = packageType;
            return true;
        }

        public void SetSyntex(string syntax)
        {
            Syntax = syntax;
        }

        public void SetProtoBuffData(StringBuilder messageLines)
        {
            ProtoBufData = messageLines;
        }
    }
}