using System;
using System.Collections.Generic;

namespace BlessBuddy.Core.Engine.Factories
{
    internal static class MemoryObjectFactory
    {
        private static readonly Dictionary<IntPtr, MemoryObject> PointerTable = new Dictionary<IntPtr, MemoryObject>();

        private static readonly Dictionary<Type, IFactory<MemoryObject>> Factories = new Dictionary
            <Type, IFactory<MemoryObject>>
            {
                {typeof(UObject), new UObjectFactory() },
                {typeof(FNameEntry), new FNameEntryFactory() },
                {typeof(UField), new UFieldFactory()},
                {typeof(UStruct), new UStructFactory()},
            };

        

        public static T CreateDeterminedObject<T>(IntPtr pointer) where T : MemoryObject
        {
            MemoryObject defaultObject;
            if (PointerTable.TryGetValue(pointer, out defaultObject) && defaultObject is T)
            {
                return (T) defaultObject;
            }

            IFactory<MemoryObject> defaultFactory;
            if (Factories.TryGetValue(typeof(T), out defaultFactory))
            {
                return (T)(PointerTable[pointer] = defaultFactory.CreateObject(pointer));
            }

            throw new ArgumentException($"There is no provided factory for {typeof(T)}");
        }

        private interface IFactory<out T> where T: MemoryObject
        {
            T CreateObject(IntPtr pointer);
        }


        private class UObjectFactory : IFactory<UObject>
        {
            public UObject CreateObject(IntPtr pointer)
            {
                return new UObject(pointer);
            }
        }

        private class FNameEntryFactory : IFactory<FNameEntry>
        {
            public FNameEntry CreateObject(IntPtr pointer)
            {
                return new FNameEntry(pointer);
            }
        }

        private class UFieldFactory : IFactory<UField>
        {
            public UField CreateObject(IntPtr pointer)
            {
                return new UField(pointer);
            }
        }

        private class UStructFactory : IFactory<UStruct>
        {
            public UStruct CreateObject(IntPtr pointer)
            {
                return new UStruct(pointer);
            }
        }
    }
}
