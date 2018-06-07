using ContentCenter.Managers;
using ContentCenter.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace ContentCenter.Test
{
    public class CleanContent
    {
        StoregeContentManager storegeContentManager = null;

        public CleanContent()
        {
            storegeContentManager = StoregeContentManager.GetInstance();
            Init();
        }

        private void Init()
        {
            string path = @"C:\Users\spegov\Downloads\video_ru (1).mp4";
            FileInfo fileInfo = new FileInfo(path);

            storegeContentManager.InsertUser(new Models.Storege
            {
                Data = File.ReadAllBytes(path),                
                //Date = DateTime.Now,
                Key = "Dfsdfs546",
                Name = fileInfo.Name,
                TypeFile = fileInfo.Extension
            });
            storegeContentManager.InsertUser(new Models.Storege
            {
                Data = File.ReadAllBytes(path),
                //Date = DateTime.Now.AddMinutes(-7),
                Key = "dddvdvd9",
                Name = fileInfo.Name,
                TypeFile = fileInfo.Extension
            });
            storegeContentManager.InsertUser(new Models.Storege
            {
                Data = File.ReadAllBytes(path),
                Key = "Dfswqwdqdfs546",
                //Date = DateTime.Now.AddMinutes(-3),
                Name = fileInfo.Name,
                TypeFile = fileInfo.Extension
            });
        }

        [Fact(DisplayName = "Проверка на очистку данных из памяти")]
        public void Cleaner()
        {
            object obj = new object();
            Dictionary<string, Storege> _storege = storegeContentManager.Storeges;

            bool result = false; 

            try
            {
                lock (obj)
                {
                    List<KeyValuePair<string, Storege>> pair
                            = _storege.ToList();

                    for (int i = 0; i < pair.Count(); i++)
                    {
                        _storege.Remove(pair[i].Key);
                    }
                }
                result = true;

            }
            catch (Exception ex)
            {
                result = false;
            }

            Assert.True(result);
        }
    }
}
