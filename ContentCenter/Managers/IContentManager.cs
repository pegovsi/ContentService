using ContentCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentCenter.Managers
{
    public interface IContentManager
    {
        Task<FileInformation> GetPathContentAsync(string key);
    }
}
