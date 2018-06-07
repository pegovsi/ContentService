using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContentCenter.Services
{
    public interface ICleanContent
    {
        void Start(int period);
    }
}
