using System;

namespace BlessBuddy.Core
{
    public class PerFrameCachedValue<T> : CachedValue<T>
    {
        private uint _frameCount = uint.MaxValue;

        public PerFrameCachedValue(Func<T> producerFunc) : base(producerFunc)
        {
        }

        protected override bool UpdateRequires(bool force)
        {
            var globalFrameCount = BlessEngine.FrameCount;
            if (! (globalFrameCount != _frameCount | force))
                return false;
            _frameCount = globalFrameCount;
            return true;
        }
    }
}
