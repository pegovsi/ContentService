using ContentCenter.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ContentCenter.Managers
{
    public class StoregeContentManager
    {
        private static readonly Lazy<StoregeContentManager> lazy =
                      new Lazy<StoregeContentManager>(() => new StoregeContentManager());

        public Dictionary<string, Storege> Storeges { get; private set; }

        private StoregeContentManager()
        {
            Storeges = new Dictionary<string, Storege>();
        }

        public static StoregeContentManager GetInstance() => lazy.Value;
        
        public void InsertUser(Storege storege)
        {
            Storeges.Add(storege.Key, storege);
        }
    }
}
