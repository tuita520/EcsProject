namespace ProtocolBuild.Generator.Core
{
    public class ConstData
    {
        public const string SYNTAX_KEY = @"syntax";
        public const string SYNTAX = @"""proto3""";

        public const string PACKAGE_KEY = @"package";
        
        public const string PACKAGE_NAME = @"Message.Protocol";
        
        public const string FILE_NAME_SERVER_PROTO = @"ServerProtocol.proto";
        public const string FILE_NAME_CLIENT_PROTO = @"ClientProtocol.proto";
        
        public const string CLIENT_MSG_PATH = @"../../ClientProtocol";
        public const string SERVER_MSG_PATH = @"../../ServerProtocol";
        
        /// <summary>
        /// 注释行
        /// </summary>
        public const string ANNOTAION = @"//";

        public static string MESSAGE_KEY = @"message";
    }
    
    
}