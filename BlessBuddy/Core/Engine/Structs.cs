using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace BlessBuddy.Core.Engine
{
    [NativeCppClass]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3F
    {
        public float x;
        public float y;
        public float z;
    }
}