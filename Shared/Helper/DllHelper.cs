using System.IO;
using System.Reflection;

namespace RDHelper
{
    public class DllHelper
    {
        public static Assembly GetServerModuleAssembly()
        {
            byte[] dllBytes = File.ReadAllBytes("./Server.Module.dll");
            byte[] pdbBytes = File.ReadAllBytes("./Server.Module.pdb");
            Assembly assembly = Assembly.Load(dllBytes, pdbBytes);
            return assembly;
        }
        
        public static Assembly GetClientModuleAssembly()
        {
            byte[] dllBytes = File.ReadAllBytes("./Client.Module.dll");
            byte[] pdbBytes = File.ReadAllBytes("./Client.Module.pdb");
            Assembly assembly = Assembly.Load(dllBytes, pdbBytes);
            return assembly;
        }
    }
}