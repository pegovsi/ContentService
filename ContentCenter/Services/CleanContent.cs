using ContentCenter.Managers;
using ContentCenter.Models;
using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ContentCenter.Services
{
    public class CleanContent : ICleanContent
    {
        static object obj = new object();
        TimerCallback tc = null;
        Timer timer = null;

        static StoregeContentManager storegeContentManager = null;

        public CleanContent()
        {
            tc = new TimerCallback(Cleaner);
            storegeContentManager = StoregeContentManager.GetInstance();
        }

        private static void Cleaner(object state)
        {
            Log.Information($"**************Show video in memory: {DateTime.Now.ToString()}****************");
            Dictionary<string, Storege> _storege = storegeContentManager.Storeges;

            try
            {
                lock (obj)
                {
                    List<KeyValuePair<string, Storege>> pair
                            = _storege.Where(x => x.Value.Date <= DateTime.Now.AddMinutes(-5)).ToList();

                    for (int i = 0; i < pair.Count(); i++)
                    {
                        _storege.Remove(pair[i].Key);
                        Log.Information($"This is video content deleted from memory by Key: {pair[i].Key} - Date:{pair[i].Value.Date}");
                    }
                }                
            }
            catch (Exception ex)
            {
                Log.Error($"Error delete content from memory. Error: {ex.Message}");
            }            
        }

        public void Start(int period)
        {
            period = period == 0 ? 60000 : period;
            timer = new Timer(tc, obj, 0, period);
        }
    }
}
