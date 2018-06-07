using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ContentCenter.Helpers;
using ContentCenter.Models;
using ContentCenter.Services;

namespace ContentCenter.Managers
{
    public class MemoryContentManager: IContentManager
    {
        private readonly ITokenManager _tokenManager;
        private StoregeContentManager storegeContentManager = StoregeContentManager.GetInstance();
        public MemoryContentManager(ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;           
        }

        public async Task<FileInformation> GetPathContentAsync(string key)
        {
            string path = string.Empty;
            TypeContent typeContent = TypeContent.none;
            string file_type = string.Empty;
            string file_name = string.Empty;

            byte[] _data = new byte[0];
            FileInfo _fileInfo = null;
            FileTypeInfo fileType = null;

            Dictionary<string, string> tokensMemory = _tokenManager.GetTokens();

            path = tokensMemory.GetValueOrDefault(key);
           // path = @"C:\Users\spegov\Downloads\video_ru (1).mp4";
            if (path == null)
                return new FileInformation
                {
                    Error = Error.NotFound,
                    FileName = string.Empty,
                    Path = path,
                    Type = string.Empty,
                    FullName = string.Empty,
                    TypeContent = typeContent
                };

            Storege storege = storegeContentManager.Storeges.GetValueOrDefault(key);
            if (storege == null)
            {
                _fileInfo = new FileInfo(path);
                if (!_fileInfo.Exists)
                    return new FileInformation
                    {
                        Error = Error.NotFound,
                        FileName = string.Empty,
                        Path = path,
                        Type = string.Empty,
                        FullName = string.Empty,
                        TypeContent = typeContent
                    };

                _data = await File.ReadAllBytesAsync(_fileInfo.FullName);
                fileType = FileHelper.GetContentType(_fileInfo.Extension);
                storege = new Storege
                {
                    Key = key,
                    Data = _data,
                    Name = _fileInfo.Name,
                    TypeFile = fileType.TypeFileString
                };

                storegeContentManager.InsertUser(storege);
                return new FileInformation
                {
                    Error = Error.None,
                    FileName = _fileInfo.Name,
                    Path = path,
                    Data = _data,
                    Type = fileType.TypeFileString,
                    FullName = _fileInfo.Name,
                    TypeContent = fileType.TypeContent
                };
            }
            else
            {   
                return new FileInformation
                {
                    Error = Error.None,
                    FileName = storege.Name,
                    Path = string.Empty,
                    Data = storege.Data,
                    Type = storege.TypeFile,
                    FullName = storege.Name,
                    TypeContent = TypeContent.video
                };
            }
        }
    }
}
