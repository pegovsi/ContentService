using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentCenter.Models
{
    public class Transport
    {
        public TypeMessage TypeMessage { get; set; }
        public string Message { get; set; }
        public string Method { get; set; }
    }
    public class TypeMessage
    {
        public string type { get; set; }
        public string id { get; set; }
        public string SenderId { get; set; }
        public List<Command> commands { get; set; } = new List<Command>();
    }

    public class Command
    {
        public string command { get; set; }
        public int priority { get; set; }
        public DateTime date_post { get; set; }
        public string param { get; set; }
    }
}
