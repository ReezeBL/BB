using System;
using System.Collections.Generic;

namespace BlessBuddy.Core.Engine
{
    public abstract class MemoryObject : IEquatable<MemoryObject>
    {
        public IntPtr BaseAddress { get; private set; }

        public virtual bool IsValid => BaseAddress != IntPtr.Zero;

        protected MemoryObject(IntPtr baseAddress)
        {
            BaseAddress = baseAddress;
        }

        public bool Equals(MemoryObject other)
        {
            return other != null && BaseAddress == other.BaseAddress;
        }
        
        public static implicit operator IntPtr(MemoryObject @object)
        {
            return @object.BaseAddress;
        }

        public bool UpdatePointer(IntPtr ptr)
        {
            if (BaseAddress == ptr)
                return false;
            BaseAddress = ptr;
            OnPointerChanged();
            return true;
        }

        protected virtual void OnPointerChanged() { }
    }
}