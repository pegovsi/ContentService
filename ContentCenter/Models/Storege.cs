using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentCenter.Models
{
    public class Storege
    {
        public string Key { get; set; }

        private byte[] data = default(byte[]);
        public byte[] Data
        {
            get
            {
                Date = DateTime.Now;
                return data;
            }
            set
            {
                data = value;
            }
        }
        public string Name { get; set; }
        public string TypeFile { get; set; }
        public DateTime Date { get; private set; }
    }
}
