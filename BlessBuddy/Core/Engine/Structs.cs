using System;
using System.Runtime.InteropServices;

namespace BlessBuddy.Core.Engine
{
    [StructLayout(LayoutKind.Sequential)]
    public struct UArray
    {
        public IntPtr ArrayPtr;
        public int ElementsCount;
        public int MaxElements;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3F
    {
        public float x;
        public float y;
        public float z;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct FNameEntry
    {
        [FieldOffset(0)]
        public byte[] buffer0;
        [FieldOffset(0x14)]
        public char name;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct UObject
    {
        [FieldOffset(0)] public byte[] buffer0;
        [FieldOffset(0x48)] public int fNameOffset;
    }
}