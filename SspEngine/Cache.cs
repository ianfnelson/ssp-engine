using System;

namespace SspEngine
{
    public abstract class Cache
    {
        private static Cache current;

        static Cache()
        {
            current = new AspNetCache();
        }

        public static Cache Current
        {
            get { return current; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                current = value;
            }
        }

        public abstract int Count { get; }

        public abstract void Insert(string key, object value);

        public abstract void Insert(string key, object value, TimeSpan timeToLive, bool slidingExpiration);

        public abstract object Get(string key);

        public abstract void Remove(string key);

        public static void ResetToDefault()
        {
            current = new AspNetCache();
        }
    }
}