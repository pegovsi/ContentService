using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentCenter.Models
{
    public class SQLServer
    {
        public string ConnectionStringPG { get; set; }
        public string QueryPG { get; set; }
        public string QueryTokenPG { get; set; }
        public string ConnectionStringMS { get; set; }
        public string QueryMS { get; set; }
        public string QueryTokenMS { get; set; }
        public Provider Provider { get; set; }
    }


    public enum Provider
    {
        pg = 0,
        ms = 1
    }
}
