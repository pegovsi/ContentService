using System.Collections.Generic;

namespace ContentCenter.Services
{
    public interface ITokenManager
    {
        Dictionary<string, string> GetTokens();
        void Insert(Dictionary<string, string> tokens);
    }
}
