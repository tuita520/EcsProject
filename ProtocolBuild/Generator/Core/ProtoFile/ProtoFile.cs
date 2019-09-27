///************************************************************************ 
// * 项目名称 :  ProtocolGenerator.ProtoBuf       
// * 类 名 称 :  ProtoBufGenerator 
// * 类 描 述 :  生成.proto文件
// * 版 本 号 :  v1.0.0.0  
// * 说    明 :      
// * 作    者 :  FReedom 
// * 创建时间 :  2018/4/20 星期五 18:49:43 
// * 更新时间 :  2018/4/20 星期五 18:49:43 
//************************************************************************ 
// * Copyright @ BoilingBlood 2018. All rights reserved. 
//************************************************************************/
//

using System;
using System.Collections.Generic;
using System.Text;
using RDLog;
using Utility;

namespace ProtocolBuild.Generator.Core
{
    public class ProtoFile
    {
        public string FullName { get;private set; }
        public string Name { get; private set; }
        public string Syntax { get; private set; }
        
        public string PackageName = ConstData.PACKAGE_NAME;
        /// <summary>
        /// key msgname value codefilename
        /// </summary>
        private readonly Dictionary<string,string> MsgNameDic = new Dictionary<string, string>();
        /// <summary>
        /// key msgId value codefilename
        /// </summary>
        private readonly Dictionary<string,string> MsgIdDic = new Dictionary<string, string>();
        
        public readonly StringBuilder MsgData = new StringBuilder();

        public void SetFileName(string fileName)
        {
            Name = fileName;
            switch (fileName)
            {
                case ConstData.FILE_NAME_CLIENT_PROTO:
                    FullName = PathUtil.PathCombine(ConstData.CLIENT_MSG_PATH, fileName);
                    break;
                case ConstData.FILE_NAME_SERVER_PROTO:
                    FullName = PathUtil.PathCombine(ConstData.SERVER_MSG_PATH, fileName);
                    break;
            }
        }
                            
        public void SetSyntax(string codeFileSyntax)
        {
            Syntax = codeFileSyntax;
        }
        public bool LoadCodeFile(CodeFile codeFile)
        {
            if (!LoadMsgName(codeFile.NameIdDic,codeFile.Name))
            {
                return false;
            }

            if (!LoadMsgId(codeFile.IdNameDic,codeFile.Name))
            {
                return false;
            }
            MsgData.Append(codeFile.ProtoBufData);
            return true;
        }

        private bool LoadMsgName(Dictionary<string, string> nameDic, string codeFileName)
        {
            foreach (var (msgName, _) in nameDic)
            {
                if (MsgNameDic.TryGetValue(msgName, out var filename))
                {
                    Log.Error($" msg name {msgName} repeated in files {filename} and {codeFileName}");
                    return false;
                }
                MsgNameDic.Add(msgName,codeFileName);
            }
            return true;
        }

        private bool LoadMsgId(Dictionary<string, string> idDic, string codeFileName)
        {
            foreach (var (msgId, _) in idDic)
            {
                if (MsgIdDic.TryGetValue(msgId,out var filename))
                {
                    Log.Error($" msg id {msgId} repeated in files {filename} and {codeFileName}");
                    return false;
                }
                MsgIdDic.Add(msgId,codeFileName);
            }
            return true;
        }

        public void GenerateProtoFile()
        {
            StringBuilder context = new StringBuilder();
            context.Append($"syntax = {Syntax};");
            context.Append(Environment.NewLine);
            context.Append($"{ConstData.PACKAGE_KEY} {PackageName};");
            context.Append(Environment.NewLine);
            context.Append(MsgData);
            FileUtil.WriteToFile(context, FullName);
        }
    }
}
