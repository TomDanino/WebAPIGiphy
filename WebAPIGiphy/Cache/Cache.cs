using System;
using System.Collections.Generic;

namespace WebAPIGiphy
{
    //I wasn't sure if we can work with a concrete object inside a web api, becuase the class controller class instantiated each time someone call it, so I worked with a static object.
    //If it was possible, I would change it to implement an interface with "Retrieve" and "Insert" methods, to add flexibility in case we want to change it in the future to a different data structure.
    public static class Cache
    {
        private static object lockObj = new object();
        private static Dictionary<string, CacheObject> cacheDictionary = new Dictionary<string, CacheObject>();

        public static CacheObject Retrieve(string key)
        {
            try
            {
                lock (lockObj)
                {
                    return cacheDictionary[key];
                }
            }
            //As the cache only help us to retrieve data faster/cheaper, we never want it to crash our application.
            catch (Exception)
            {
                //write exception to log
                return null;
            }
        }

        public static bool Insert(string key, IEnumerable<string> value)
        {
            try
            {
                lock (lockObj)
                {
                    if (cacheDictionary.ContainsKey(key))
                    {
                        cacheDictionary[key].urls = value;
                        cacheDictionary[key].lastUpdate = DateTime.Now;
                    }

                    cacheDictionary.Add(key, new CacheObject(DateTime.Now, value));
                }
                return true;
            }
            //As the cache only help us to retrieve data faster/cheaper, we never want it to crash our application.
            catch (Exception)
            {
                //write exception to log
                return false;
            }
        }
    }
}