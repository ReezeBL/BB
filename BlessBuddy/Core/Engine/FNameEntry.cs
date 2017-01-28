using System;
using System.Collections.Generic;
using System.Text;

namespace BlessBuddy.Core.Engine
{
    public class FNameEntry : MemoryObject
    {
        private CachedValue<string> _name;

        public static readonly Dictionary<string, int> Table = new Dictionary<string, int>();

        public string Name
        {
            get
            {
                return _name ??
                       (_name =
                           new CachedValue<string>(
                               () => BlessEngine.Memory.Read(BaseAddress + 0x14, Encoding.Default, 100)));
            }
        }

        public FNameEntry(IntPtr baseAddress) : base(baseAddress)
        {
        }
    }
}