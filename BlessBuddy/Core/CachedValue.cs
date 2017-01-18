using System;

namespace BlessBuddy.Core
{
    public abstract class CachedValue<T>
    {
        private readonly Func<T> _producer;
        private T _cachedValue;
        private bool _cacheForced;

        public EventHandler OnValueChanged;

        public T Value
        {
            get
            {
                if (UpdateRequires(_cacheForced))
                {
                    _cachedValue = _producer();
                    _cacheForced = false;
                    OnValueChanged?.Invoke(this, null);
                }
                return _cachedValue;
            }
        }

        protected abstract bool UpdateRequires(bool force);

        protected CachedValue(Func<T> producerFunc)
        {
            if (producerFunc == null)
                throw new ArgumentNullException(nameof(producerFunc));
            _producer = producerFunc;
        }

        public void RequestUpdateValues()
        {
            _cacheForced = true;
        }

        public static implicit operator T(CachedValue<T> cachedValue)
        {
            return cachedValue.Value;
        }
    }
}
