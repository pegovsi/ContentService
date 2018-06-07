using System.Collections.Generic;

namespace ContentCenter.Services
{
    public class TokenManager : ITokenManager
    {
        TokenManagerStorage _tokenManagerStorage = TokenManagerStorage.GetInstance();
        public Dictionary<string, string> GetTokens()
        {
            return _tokenManagerStorage.Tokens;
        }

        public void Insert(Dictionary<string, string> tokens)
        {
            _tokenManagerStorage.InsertTokens(tokens);
        }
    }
}
