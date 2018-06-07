using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentCenter.Models
{
    public class FileInformation
    {
        public string Path { get; set; }
        public string @Type { get; set; }
        public TypeContent TypeContent { get; set; }
        public string FileName { get; set; }
        public string FullName { get; set; }
        public byte[] Data { get; set; }
        
        public Error Error { get; set; }
    }

    public enum TypeContent
    {
        image = 0,
        video = 1,
        none = 2
    }
}
