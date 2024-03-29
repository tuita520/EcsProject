﻿using System;
using System.ComponentModel;

namespace Frame.Core.Base
{
    public abstract class AComponent: ISupportInitialize,IDisposable
    {
        public long Id { get; set; }
        public AComponent Parent { get; set; }
        public bool IsDisposed { get; set; }
        
        public AComponent()
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
        
        public virtual void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            IsDisposed = true;
            App.Inst.EventSystem.Destroy(this);
            App.Inst.EventSystem.Remove(Id);
            Id = 0;
            //TODO:BOIL dispose
        }
        
        public T GetParent<T>() where T : AComponent
        {
            return this.Parent as T;
        }

    }
}