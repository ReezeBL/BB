using System;

namespace BlessBuddy.Core.Engine
{
    class UProperty : UField
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
}
