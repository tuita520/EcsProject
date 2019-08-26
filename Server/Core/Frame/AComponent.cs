using System;
using System.ComponentModel;

namespace Core.Frame
{
    public abstract class AComponent:ISupportInitialize,IDisposable
    {
        public long Id { get; set; }
        public AComponent Parent { get; set; }

        public bool IsDisposed { get; set; }
        
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
            return this.Parent as T;
        }

   
    }
}