using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace replicationToAmazonS3.Core
{
    public class ErrorSaveObjectResult
    {
        public string Message { get; protected set; }
        public string FilePath { get; protected set; }
        public Stream FileStream { get; protected set; }
        public string KeyName { get; protected set; }
        public Exception Exception { get; protected set; }

        public ErrorSaveObjectResult(string message, Exception exception, string filePath, string keyName)
        {
            this.Message = message;
            this.Exception = exception;
            this.FilePath = filePath;
            this.KeyName = keyName;
        }

        public ErrorSaveObjectResult(string message, Exception exception, Stream fileStream, string keyName)
        {
            this.Message = message;
            this.Exception = exception;
            this.FileStream = fileStream;
            this.KeyName = keyName;
        }
    }
}
