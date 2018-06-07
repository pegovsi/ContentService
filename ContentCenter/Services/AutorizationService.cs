using ContentCenter.Models;
using System.Collections.Generic;
using System.Linq;

namespace ContentCenter.Services
{
    public class AutorizationService : IAutorizationService
    {
        private readonly ICoderSHA _coderSHA;
        private readonly IUsersManager _usersManager;

        public AutorizationService(ICoderSHA coderSHA, IUsersManager usersManager)
        {
            _coderSHA = coderSHA;
            _usersManager = usersManager;
        }

        public string HexString2B64String(string input) => System.Convert.ToBase64String(HexStringToHex(input));
       
        public byte[] HexStringToHex(string inputHex)
        {
            var resultantArray = new byte[inputHex.Length / 2];
            for (var i = 0; i < resultantArray.Length; i++)
            {
                resultantArray[i] = System.Convert.ToByte(inputHex.Substring(i * 2, 2), 16);
            }
            return resultantArray;
        }

        public bool IsUserFrom1C(string tokenHex)
        {
            var token = HexString2B64String(tokenHex);
            string _passwd = GetPassword(token);

            string[] arrayString = _passwd.Split(':');
            string login = arrayString[0];
            string passwd = arrayString[1];

            string hash = _coderSHA.GetSha1(passwd);
                      
            return ExecuteСomparar(login, hash);
        }

        //опросим то что в памяти от БД и сравним
        private bool ExecuteСomparar(string login, string hash)
        {
            List<UserDb> users = _usersManager.GetUsers();
            var _user = users.Find(x => x.Name.ToUpper() == login.ToUpper());

            if (_user == null)
                return false;
            if (_user.Hash.Trim('"') == hash)
                return true;
            else
                return false;
        }

        public bool IsUserFrom1CByBase64(string token)
        {
            if (string.IsNullOrEmpty(token))
                return false;

            string _passwd = GetPassword(token);

            string[] arrayString = _passwd.Split(':');
            string login = arrayString[0];
            string passwd = arrayString[1];

            string hash = _coderSHA.GetSha1(passwd);

            return ExecuteСomparar(login, hash);
        }

        private string Base64Decode(string token)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(token);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        private string GetPassword(string token) => Base64Decode(token);              
    }
}
