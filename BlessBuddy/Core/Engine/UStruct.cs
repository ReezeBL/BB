using System;
using BlessBuddy.Core.Engine.Factories;

namespace BlessBuddy.Core.Engine
{
    public class UStruct : UField
    {
        private UField _superField;
        private UField _children;
        private int? _propertySize;

        public UField SuperField
        {
            get
            {
                if (_superField == null)
                {
                    var addr = BlessEngine.Memory.Read<IntPtr>(BaseAddress + 0x78);
                    _superField = MemoryObjectFactory.CreateDeterminedObject<UField>(addr);
                }
                return _superField;
            }
        }

        public UField Children
        {
            get
            {
                if (_children == null)
                {
                    var addr = BlessEngine.Memory.Read<IntPtr>(BaseAddress + 0x80);
                    _children = MemoryObjectFactory.CreateDeterminedObject<UField>(addr);
                }
                return _children;
            }
        }

        public int PropertySize
        {
            get
            {
                if (_propertySize == null)
                {
                    _propertySize = BlessEngine.Memory.Read<int>(BaseAddress + 0x88);
                }
                return _propertySize.Value;
            }
        }

        public UStruct(IntPtr baseAddress) : base(baseAddress)
        {
        }
    }
}
