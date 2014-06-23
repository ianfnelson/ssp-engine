using System;
using System.Web;

namespace SspEngine
{
    public class AspNetCache : Cache
    {
        public override int Count
        {
            get { return HttpRuntime.Cache.Count; }
        }

        public override object Get(string key)
        {
            return HttpRuntime.Cache.Get(key);
        }

        public override void Insert(string key, object value)
        {
            if (value == null)
            {
                return;
            }

            HttpRuntime.Cache.Insert(key, value);
        }

        public override void Insert(string key, object value, TimeSpan timeToLive, bool slidingExpiration)
        {
            if (value == null)
            {
                return;
            }

            if (slidingExpiration)
            {
                HttpRuntime.Cache.Insert(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, timeToLive);
            }
            else
            {
                HttpRuntime.Cache.Insert(key, value, null, DateTime.Now.Add(timeToLive),System.Web.Caching.Cache.NoSlidingExpiration);
            }
        }

        public override void Remove(string key)
        {
            HttpRuntime.Cache.Remove(key);
        }
    }
}