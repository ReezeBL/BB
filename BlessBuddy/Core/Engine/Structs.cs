using System;
using System.Collections.Generic;
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

    [StructLayout(LayoutKind.Explicit, Size = 120)]
    internal struct FNameEntry
    {
        public static readonly Dictionary<int, string> Table = new Dictionary<int, string>();

        [FieldOffset(0)]
        public byte buffer0;
        [FieldOffset(0x14)]
        public char name;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct UObject
    {
        [FieldOffset(0)] public IntPtr VTable;
        [FieldOffset(0x48)] public int fNameOffset;
        [FieldOffset(0x50)] public IntPtr Class;
        [FieldOffset(0x58)] public IntPtr ObjectArchetype;
    }
}