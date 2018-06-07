
namespace ContentCenter.Services
{
    public interface IAutorizationService
    {
        bool IsUserFrom1C(string token);
        bool IsUserFrom1CByBase64(string token);
    }
}
