namespace ProtocolBuild.Generator.Core
{
    public class ConstData
    {
        public const string SYNTAX_KEY = @"syntax";
        public const string SYNTAX = @"""proto3""";

        public const string PACKAGE_KEY = @"package";

        public const string Server_Package = @"Message.Server;";
        public const string Client_Package = @"Message.Client;";
        
        /// <summary>
        /// 注释行
        /// </summary>
        public const string ANNOTAION = @"//";

        public static string MESSAGE_KEY = @"message";
    }
    
    
}