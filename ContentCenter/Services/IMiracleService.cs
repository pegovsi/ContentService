using System.Threading.Tasks;

namespace ContentCenter.Services
{
    public interface IMiracleService
    {
        Task FillData();
        Task FillTokenCollection();
    }
}
