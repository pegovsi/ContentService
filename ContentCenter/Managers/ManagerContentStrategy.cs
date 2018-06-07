using ContentCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentCenter.Managers
{
    public class ManagerContentStrategy
    {
        public IContentManager _contentManager {private get; set;}

        public ManagerContentStrategy(IContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        public async Task<FileInformation> GetPathContentAsync(string key)
            => await _contentManager.GetPathContentAsync(key);
    }
}
