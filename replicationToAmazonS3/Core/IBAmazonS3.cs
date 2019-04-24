using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace replicationToAmazonS3.Core
{
    public interface IBAmazonS3
    {
        void SaveObject(Stream pObject, string keyname);
        void DeleteObject(string keyname);
        void CreateKeyName(string keyname);
    }
}
