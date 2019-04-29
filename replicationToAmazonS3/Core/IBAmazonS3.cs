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
        void SaveObject(string filePath, string keyname);
        void SaveObject(string filePath, string keyname, out ErrorSaveObjectResult result);
        void SaveObject(Stream pObject, string keyname, out ErrorSaveObjectResult result);
        void DeleteObject(string keyname);
        void CreateKeyName(string keyname);
    }
}
