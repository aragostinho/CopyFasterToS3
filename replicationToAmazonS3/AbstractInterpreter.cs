using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace replicationToAmazonS3
{
    public abstract class AbstractInterpreter
    {
        public abstract void Execute(string[] args);
        public abstract string Description();
    }
}
