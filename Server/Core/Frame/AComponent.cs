using System;
using System.ComponentModel;

namespace Core.Frame
{
    public abstract class AComponent:ISupportInitialize,IDisposable
    {
        private long id;

        public long Id
        {
            get => id;
            set => id = value;
        }

        private AComponent parent;

        public AComponent Parent
        {
            get => parent;
            set => parent = value;
        }

        protected bool IsDisposed;
        
        protected AComponent()
        {
            IsDisposed = false;
        }
        
        public void BeginInit()
        {
            throw new NotImplementedException();
        }

        public void EndInit()
        {
            throw new NotImplementedException();
        }
        
        public void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            IsDisposed = true;
            //TODO:BOIL dispose
        }
        
        public T GetParent<T>() where T : AComponent
        {
            return this.parent as T;
        }

   
    }
}