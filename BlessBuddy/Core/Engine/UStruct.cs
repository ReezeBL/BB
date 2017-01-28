using System;
using BlessBuddy.Core.Engine.Factories;

namespace BlessBuddy.Core.Engine
{
    public class UStruct : UField
    {
        private UField _superField;
        private UField _children;

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

        public UStruct(IntPtr baseAddress) : base(baseAddress)
        {
        }
    }
}
