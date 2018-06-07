using System.Threading.Tasks;
using ContentCenter.Managers;
using ContentCenter.Models;

namespace ContentCenter.Services
{
    public class ContentService : IContentService
    {
        private readonly ITokenManager _tokenManager;

        public ContentService(ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
        }
        public async Task<FileInformation> GetContentAsync(string key)
        {
            //TODO: Определить откуда забирать
            ManagerContentStrategy managerContentStrategy 
                    = new ManagerContentStrategy(new MemoryContentManager(_tokenManager));

            return await managerContentStrategy.GetPathContentAsync(key);            
        }
    }
}
