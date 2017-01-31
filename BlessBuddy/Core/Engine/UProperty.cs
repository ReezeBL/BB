using System;
using BlessBuddy.Core.Engine.Factories;

namespace BlessBuddy.Core.Engine
{
    public class UProperty : UField
    {
        private int? _propertyOffset;

        public int PropertyOffset
        {
            get
            {
                if (_propertyOffset == null)
                {
                    _propertyOffset = BlessEngine.Memory.Read<int>(BaseAddress + 0x8C);
                }
                return _propertyOffset.Value;
            }
        }

        public UProperty(IntPtr baseAddress) : base(baseAddress)
        {
        }
    }

    public class UStructProperty : UProperty
    {
        private UObject _propertyClass;

        public UObject PropertyClass
        {
            get
            {
                if (_propertyClass == null)
                {
                    var addr = BlessEngine.Memory.Read<IntPtr>(BaseAddress + 0xA8);
                    _propertyClass = MemoryObjectFactory.CreateDeterminedObject<UObject>(addr);
                }
                return _propertyClass;
            }
        }

        public UStructProperty(IntPtr baseAddress) : base(baseAddress)
        {
        }
    }

    public class UClassProperty : UStructProperty
    {
        private UObject _metaObject;

        public UObject MetaObject
        {
            get
            {
                if (_metaObject == null)
                {
                    var addr = BlessEngine.Memory.Read<IntPtr>(BaseAddress + 0xB0);
                    _metaObject = MemoryObjectFactory.CreateDeterminedObject<UObject>(addr);
                }
                return _metaObject;
            }
        }

        public UClassProperty(IntPtr baseAddress) : base(baseAddress)
        {
        }
    }
}
