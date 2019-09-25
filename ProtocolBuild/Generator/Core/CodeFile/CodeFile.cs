using System.Collections.Generic;
using System.Text;
using RDLog;

namespace ProtocolBuild.Generator.Core
{
    public class CodeFile
    {
        public string FullName { get; set; }
        public string Name { get; set; }

        private string syntax { get; set; }
        private readonly List<string> _packetNameList = new List<string>();

        private readonly Dictionary<string, string> _idNameDic = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _nameIdDic = new Dictionary<string, string>();

        private StringBuilder ProtoBufData { get; set; }

        public bool AddMessageNameId(string msgName, string msgId = null)
        {
            if (_nameIdDic.TryGetValue(msgName, out var id))
            {
                Log.Error($"file {Name} got an wrong msg name : {msgName} ");
                return false;
            }

            if (_idNameDic.TryGetValue(msgId, out var name))
            {
                Log.Error($"file {Name} got an wrong msg id : {msgId}");
                return false;
            }

            if (!string.IsNullOrEmpty(msgId))
            {
                _idNameDic.Add(msgId, msgName);
            }

            _nameIdDic.Add(msgName, msgId);
            return true;
        }


        public bool AddProtoPackageName(string packageName)
        {
            if (_packetNameList.Contains(packageName))
            {
                return false;
            }
            _packetNameList.Add(packageName);
            return true;
        }

        public void SetSyntex(string syntax)
        {
            this.syntax = syntax;
        }

        public void SetProtoBuffData(StringBuilder messageLines)
        {
            ProtoBufData = messageLines;
        }
    }
}