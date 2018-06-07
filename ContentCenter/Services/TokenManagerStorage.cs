using System;
using System.Collections.Generic;
using System.Linq;

namespace ContentCenter.Services
{
    public class TokenManagerStorage
    {
        private static readonly Lazy<TokenManagerStorage> lazy =
                        new Lazy<TokenManagerStorage>(() => new TokenManagerStorage());

        public Dictionary<string, string> Tokens { get; private set; }

        private TokenManagerStorage()
        {
            Tokens = new Dictionary<string, string>();
        }

        public static TokenManagerStorage GetInstance()
        {
            return lazy.Value;
        }

        public void InsertTokens(Dictionary<string, string> tokens)
        {
            foreach (var item in tokens)
            {
                var element = Tokens.GetValueOrDefault(item.Key);
                if(element ==null)
                    Tokens.Add(item.Key, item.Value);
            }
        }
    }
}
