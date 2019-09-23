﻿/************************************************************************ 
 * 项目名称 :  ProtocolGenerator.CSharp       
 * 类 名 称 :  CSharpGenerator 
 * 类 描 述 : 
 * 版 本 号 :  v1.0.0.0  
 * 说    明 :      
 * 作    者 :  Boiling 
 * 创建时间 :  2018/4/20 星期五 19:28:28 
 * 更新时间 :  2018/4/20 星期五 19:28:28 
************************************************************************ 
 * Copyright @ BoilingBlood 2018. All rights reserved. 
************************************************************************/

namespace ProtocolGenerator.Core.CSharp
{
    public class CSharpGenerator : IGenerator
    {
        public void Generate()
        {
            CSharpClass_Id classGen = new CSharpClass_Id();
            classGen.Generate();
        }
        public void Generate(Data data)
        {
            CSharpClass_IdGenerator classGen = new CSharpClass_IdGenerator(data);
            classGen.Generate();
            CSharpClass_Proto protoGen = new CSharpClass_Proto(data);
            //protoGen.GenerateProto_Old();
            //protoGen.GenerateProto_Net400();
            protoGen.GenerateProto();
        }
    }
}
