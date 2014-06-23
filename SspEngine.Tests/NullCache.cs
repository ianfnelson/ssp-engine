using System;

namespace SspEngine.Tests
{
    public class NullCache : Cache
    {
        public override int Count
        {
            get { return 0; }
        }

        public override void Insert(string key, object value)
        {
        }

        public override void Insert(string key, object value, TimeSpan timeToLive, bool slidingExpiration)
        {
        }

        public override object Get(string key)
        {
            return null;
        }

        public override void Remove(string key)
        {
        }
    }
}