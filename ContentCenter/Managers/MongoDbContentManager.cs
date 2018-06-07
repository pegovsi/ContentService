using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContentCenter.Models;

namespace ContentCenter.Managers
{
    public class MongoDbContentManager : IContentManager
    {
        public MongoDbContentManager()
        {

        }
        public async Task<FileInformation> GetPathContentAsync(string key)
        {
            return new FileInformation();
        }
    }
}
