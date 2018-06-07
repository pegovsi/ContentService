using System;
using System.Security.Cryptography;
using System.Text;

namespace ContentCenter.Services
{
    public class CoderSHA : ICoderSHA
    {
        public string GetSha1(string text)
        {
            string cahe = string.Empty;
            var str1 = GetCacheSha1(text);           
            cahe = $"{str1}";
            return cahe;
        }

        private string GetCacheSha1(string str)
        {
            string cahe = string.Empty;

            byte[] buffer = Encoding.UTF8.GetBytes(str);
            SHA1 sha1 = SHA1.Create();
            byte[] _bufferText = sha1.ComputeHash(buffer);
            cahe = Convert.ToBase64String(_bufferText);

            return cahe;
        }
    }
}
