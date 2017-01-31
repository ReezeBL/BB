using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BlessBuddy.Core.Engine
{
    public class UFunction : UStruct
    {
        private int? _functionFlags;
        private int? _functionNameOffset;
        private byte? _numOfParams;

        private IntPtr? _functionPointer;
        private string _name;

        public int FunctionFlags
        {
            get
            {
                if (_functionFlags == null)
                {
                    _functionFlags = BlessEngine.Memory.Read<int>(BaseAddress + 0xD0);
                }
                return _functionFlags.Value;
            }
        }

        public int FunctionNameOffset
        {
            get
            {
                if (_functionNameOffset == null)
                {
                    _functionNameOffset = BlessEngine.Memory.Read<int>(BaseAddress + 0xD8);
                }
                return _functionNameOffset.Value;
            }
        }

        public string FunctionName => _name ?? (_name = BlessEngine.GNames[FunctionNameOffset].Name);

        public IntPtr FunctionPtr
        {
            get
            {
                if (_functionPointer == null)
                {
                    _functionPointer = BlessEngine.Memory.Read<IntPtr>(BaseAddress + 0xF0);
                }
                return _functionPointer.Value;
            }
        }

        public byte ParametersCount
        {
            get
            {
                if (_numOfParams == null)
                {
                    _numOfParams = BlessEngine.Memory.Read<byte>(BaseAddress + 0xE1);
                }
                return _numOfParams.Value;
            }
        }

        public UFunction(IntPtr baseAddress) : base(baseAddress)
        {
        }
    }
}
