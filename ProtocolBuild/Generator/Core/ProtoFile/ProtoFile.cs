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

using System.Collections.Generic;
using System.Text;

namespace ProtocolBuild.Generator.Core
{
    public class ProtoFile
    {
        public string FullName { get; set; }
        public string Name { get; set; }

        private string syntax { get; set; }
        private string packageName { get; set; }
        /// <summary>
        /// key msgname value filename
        /// </summary>
        private Dictionary<string,string> msgNameDic = new Dictionary<string, string>();
        
        private StringBuilder msgData = new StringBuilder();
    }
}
