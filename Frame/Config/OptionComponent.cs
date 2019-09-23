using System;
using Frame.Core.Base;
using Frame.Core.Base.Attributes;
using Frame.Core.Enumerate;
using RDLog;

namespace Frame.Config
{
    public class OptionComponent : AComponent
    {
        public Options Options { get; private set; }

        public void Awake(string[] args)
        {
            Options = new Options();
            if (args.Length > 0)
            {
                if (Enum.IsDefined(typeof(AppType), args[0]))
                {
                    Options.AppType = (AppType) Enum.Parse(typeof(AppType), args[0], true);
                }
                else
                {
                    Log.Error("Uh oh!");
                    return;
                }
            }

            if (args.Length > 1)
            {
                Options.AreaId = int.Parse(args[1]);
            }

            if (args.Length > 2)
            {
                Options.SubId = int.Parse(args[2]);
            }

        }
    }

    [System]
    public class OptionComponentSystem : AAwakeSystem<OptionComponent, string[]>
    {
        protected override void Awake(OptionComponent self, string[] a)
        {
            self.Awake(a);
        }
    }
}