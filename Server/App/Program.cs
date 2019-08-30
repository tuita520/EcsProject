using System;
using Frame.Core.Base;
using RDLog;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                App.Inst.Init();
                App.Inst.Run();
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
        }
    }
}
