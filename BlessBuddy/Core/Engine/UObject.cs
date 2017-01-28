using System;
using System.Runtime.InteropServices;
using BlessBuddy.Core.Engine.Factories;

namespace BlessBuddy.Core.Engine
{
    public class UObject : MemoryObject
    {
        private string _name;
        private int? _nameId;
        private UObject _outer;
        private UStruct _class;
        private UObject _archetype;

        public int NameId
        {
            get
            {
                if (_nameId == null)
                    _nameId = BlessEngine.Memory.Read<int>(BaseAddress + 0x48);
                return _nameId.Value;
            }
        }

        public string Name => _name ?? (_name = BlessEngine.GNames[NameId].Name);

        public UStruct Class
        {
            get
            {
                if (_class == null)
                {
                    var addr = BlessEngine.Memory.Read<IntPtr>(BaseAddress + 0x50);
                    _class = MemoryObjectFactory.CreateDeterminedObject<UStruct>(addr);
                }
                return _class;
            }
        }

        public UObject ArcheType
        {
            get
            {
                if (_archetype == null)
                {
                    var addr = BlessEngine.Memory.Read<IntPtr>(BaseAddress + 0x58);
                    _archetype = MemoryObjectFactory.CreateDeterminedObject<UObject>(addr);
                }
                return _archetype;
            }
        }

        public UObject Outer
        {
            get
            {
                if (_outer == null)
                {
                    var addr = BlessEngine.Memory.Read<IntPtr>(BaseAddress + 0x40);
                    _outer = MemoryObjectFactory.CreateDeterminedObject<UObject>(addr);
                }
                return _outer;
            }
        }

        public UObject(IntPtr baseAddress) : base(baseAddress)
        {
        }
    }
}