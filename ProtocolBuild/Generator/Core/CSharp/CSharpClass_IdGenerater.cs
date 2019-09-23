﻿/************************************************************************ 
 * 项目名称 :  ProtocolGenerator.CSharp       
 * 类 名 称 :  CSharpClass_IdGenerator 
 * 类 描 述 :  生成协议Id的.cs文件
 * 版 本 号 :  v1.0.0.0  
 * 说    明 :      
 * 作    者 :  Boiling 
 * 创建时间 :  2018/4/20 星期五 17:45:39 
 * 更新时间 :  2018/4/20 星期五 17:45:39 
************************************************************************ 
 * Copyright @ BoilingBlood 2018. All rights reserved. 
************************************************************************/
using System.Collections.Generic;
using System.Text;

namespace ProtocolGenerator.Core.CSharp
{
    public class CSharpClass_IdGenerator: AbstractFileModel
    {
        CSharpCodeGenerator _generater = new CSharpCodeGenerator();
        string className = "IdGenerator";
        Data _data = null;
        public CSharpClass_IdGenerator(Data data)
        {
            _data = data;
            string tempPath = _data.ProtoPackageName.Replace('.', '\\');
            m_filePath = Program.OutputPath + @"\CSharp\"+ tempPath+@"\";
            m_fileName = _data.ProtoFileKey;
            m_fileSuffix = "IdGenerator.cs";
            className = m_fileName + className;
        }

        private StringBuilder GenerateIdKey(string key)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(CSharpClass_Id.msgIdPackageName);
            sb.Append(".Id<");
            sb.Append(key);
            sb.Append(">.Value=");
            return sb;
        }

        private StringBuilder GenerateIdValue(string value)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(value);
            return sb;
        }

        protected override StringBuilder GenerateClassCode()
        {
            StringBuilder fileContent = new StringBuilder();
            StringBuilder fileComments = _generater.ClassCommentsFrame();
            StringBuilder fileIncludeHeads = _generater.IncludeHeadFrame(null);

            List<StringBuilder> attrs = new List<StringBuilder>();
            foreach (var item in _data.DicName2Id)
            {
                StringBuilder sbClassAttrFrame = _generater.AttrFrame(GenerateIdKey(item.Key).ToString(), GenerateIdValue(item.Value).ToString());
                attrs.Add(sbClassAttrFrame);
            }

            StringBuilder sbClassMethodFrame = _generater.MethodFrame("static void", "GenerateId()", attrs);
            List<StringBuilder> methods = new List<StringBuilder>();
            methods.Add(sbClassMethodFrame);

            StringBuilder sbClassBodyFrame = _generater.ClassFrame("class " + className, null, methods);
            StringBuilder sbNameSpaceFrame = _generater.NameSpaceFrame(_data.ProtoPackageName, sbClassBodyFrame);

            fileContent.Append(fileComments);
            fileContent.Append(fileIncludeHeads);
            fileContent.Append(sbNameSpaceFrame);
            return fileContent;
        }
    }
}
