namespace ProtocolGenerator.Core
{
    public class ConstData
    {
        public const string SYNTAX_KEY = @"syntax";
        
        public const string SYNTAX = @"""proto3""";

        public const string PACKAGE_KEY = @"package";

        public const string OPTION_KEY = @"option";

        public const string IMPORT_KEY = @"import";
        
        public const string ENUM_KEY = @"enum"; 

        public static string MESSAGE_KEY = @"message";

        public const string ANNOTAION_KEY = @"//";

        public const string PROTOCOL_PATH = @"..\..\..\ProtocolBuild";
        
        public const string SERVERMSG_OUTPUT_PATH = @"..\..\..\ServerProtocol";
        public const string CLIENTMSG_OUTPUT_PATH = @"..\..\..\ClientProtocol";

        public const string CSHARP_NAMESPACE_KEY = @"csharp_namespace";
    }
    
    
}