using System;
using System.Collections.Generic;

namespace WebAPIGiphy
{
    public class CacheObject
    {
        public DateTime lastUpdate;
        public IEnumerable<string> urls;

        public CacheObject(DateTime time, IEnumerable<string> urls)
        {
            this.lastUpdate = time;
            this.urls = urls;
        }
    }
}