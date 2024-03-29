﻿using RDLog;
using Utility;

namespace ProtocolGenerator.Core
{
    public class ProtoFileGenerator
    {
        private ProtoFile _protoFile;
        public ProtoFileGenerator(ProtoFile protoFile)
        {
            _protoFile = protoFile;
        }
        
        public void Generate()
        {
            if (FileUtil.WriteToFile(_protoFile.ProtoBuffContext,_protoFile.FullName))
            {
                Log.Info($"{_protoFile.FullName} generate success");
                return;
            }
            Log.Error($"{_protoFile.FullName} generate fail");
        }
    }


}