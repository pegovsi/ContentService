using ContentCenter.Models;
using System.Threading.Tasks;

namespace ContentCenter.Services
{
    public interface IContentService
    {
        Task<FileInformation> GetContentAsync(string key);
    }
}
