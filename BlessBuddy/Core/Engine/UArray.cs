using System;
using System.Collections;
using System.Collections.Generic;

namespace BlessBuddy.Core.Engine
{
    
    public class UArray<T> : MemoryObject, IEnumerable<T>
    {
        private CachedValue<IntPtr> _arrayPtr;
        private CachedValue<int> _elementsCount;
        private CachedValue<int> _maxElements;
        private CachedValue<T>[] _array;

        private readonly Func<IntPtr, T> _pointerSolver;

        public IntPtr ArrayPtr
        {
            get
            {
                return
                    _arrayPtr ?? (_arrayPtr =
                        new CachedValue<IntPtr>(() => BlessEngine.Memory.Read<IntPtr>(BaseAddress)));
            }
        }

        public int ElementsCount
        {
            get
            {
                return
                    _elementsCount ?? (_elementsCount =
                        new PerFrameCachedValue<int>(() => BlessEngine.Memory.Read<int>(BaseAddress + 0x8)));
            }
        }

        public int MaxElements
        {
            get
            {
                if (_maxElements == null)
                {
                    _maxElements = new PerFrameCachedValue<int>(() => BlessEngine.Memory.Read<int>(BaseAddress + 0xC));
                    _maxElements.OnValueChanged += OnMaxElementsChanged;
                }
                return _maxElements;
            }
        }

        public UArray(IntPtr baseAddress, Func<IntPtr, T> pointerSolver = null) : base(baseAddress)
        {
            _pointerSolver = pointerSolver;
        }

        public T this[int n]
        {
            get
            {
                if (_array == null)
                    _array = new CachedValue<T>[MaxElements];
                if(ArrayPtr == IntPtr.Zero)
                    throw new AccessViolationException();
                if (_array[n] == null)
                {
                    if (_pointerSolver != null)
                    {
                        var addr = BlessEngine.Memory.Read<IntPtr>(ArrayPtr + n * 8);
                        _array[n] = new CachedValue<T>(() => _pointerSolver(addr));
                    }
                    else
                        _array[n] = new CachedValue<T>(() => BlessEngine.Memory.Read<T>(ArrayPtr + n * 8));
                }
                return _array[n];
            }
        }

        private void OnMaxElementsChanged(object sender, EventArgs args)
        {
            if (_array == null || _array.Length < MaxElements)
            {
                _array = new CachedValue<T>[MaxElements];
                _arrayPtr?.RequestUpdateValues();
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < ElementsCount; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}