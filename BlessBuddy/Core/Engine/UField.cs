using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlessBuddy.Core.Engine.Factories;

namespace BlessBuddy.Core.Engine
{
    public class UField : UObject
    {
        private UField _next;

        public UField(IntPtr baseAddress) : base(baseAddress)
        {
        }

        public UField Next
        {
            get
            {
                if (_next == null)
                {
                    var addr = BlessEngine.Memory.Read<IntPtr>(BaseAddress + 0x60);
                    _next = MemoryObjectFactory.CreateDeterminedObject<UField>(addr);
                }
                return _next;
            }
        }

    }
}
