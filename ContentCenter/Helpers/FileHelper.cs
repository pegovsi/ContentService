using ContentCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentCenter.Helpers
{
    public static class FileHelper
    {
        private static string GetTypeFile(string text) => text.Trim('.');
        
        public static FileTypeInfo GetContentType(string text)
        {            
            string contentType = string.Empty;
            TypeContent typeContent = TypeContent.video;

            var _typeFile = GetTypeFile(text);
            switch (_typeFile)
            {
                case "jpg":
                    contentType = $"image/{_typeFile}";
                    typeContent = TypeContent.image;
                    break;
                case "jpeg":
                    contentType = $"image/{_typeFile}";
                    typeContent = TypeContent.image;
                    break;
                case "gif":
                    contentType = $"image/{_typeFile}";
                    typeContent = TypeContent.image;
                    break;
                case "png":
                    contentType = $"image/{_typeFile}";
                    typeContent = TypeContent.image;
                    break;
                case "mp4":
                    contentType = $"video/{_typeFile}";
                    typeContent = TypeContent.video;
                    break;
                case "mpeg4":
                    contentType = $"video/{_typeFile}";
                    typeContent = TypeContent.video;
                    break;
                case "mpeg":
                    contentType = $"video/{_typeFile}";
                    typeContent = TypeContent.video;
                    break;
                case "vmk":
                    contentType = $"video/{_typeFile}";
                    typeContent = TypeContent.video;
                    break;
                case "wma":
                    contentType = $"video/{_typeFile}";
                    typeContent = TypeContent.video;
                    break;
                case "zip":
                    contentType = $"application/zip";
                    typeContent = TypeContent.video;
                    break;
                default:
                    contentType = $"";
                    typeContent = TypeContent.video;
                    break;
            }


            return new FileTypeInfo
            {
                TypeContent = typeContent ,
                TypeFileString = contentType
            };
        }
    }

    public class FileTypeInfo
    {
        public string TypeFileString { get; set; }
        public TypeContent TypeContent { get; set; }
    }
}
