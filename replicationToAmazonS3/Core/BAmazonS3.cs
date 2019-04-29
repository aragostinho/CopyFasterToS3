using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Amazon.S3.IO;

namespace replicationToAmazonS3.Core
{
    public class BAmazonS3 : IBAmazonS3
    {
        private string _awsAccessKey;
        private string _awsSecretAccessKey;
        private AmazonS3Config config;
        private bool _isS3TransferAccelerationActived = bool.Parse(ConfigurationManager.AppSettings["S3TransferAcceleration"]);
        private string _s3TransferAccelerationEndPoint = ConfigurationManager.AppSettings["S3TransferAccelerationEndPoint"];


        public string BucketName
        {
            get;
            set;
        }

        public string KeyName
        {
            get;
            set;
        }

        public BAmazonS3()
        {
            _awsAccessKey = ConfigurationManager.AppSettings["AWSAccessKey"];
            _awsSecretAccessKey = ConfigurationManager.AppSettings["AWSSecretKey"];
            this.BucketName = ConfigurationManager.AppSettings["Bucketname"];
            _s3TransferAccelerationEndPoint = _s3TransferAccelerationEndPoint.Replace("{bucketName}", this.BucketName);
            config = new AmazonS3Config();
            config.ServiceURL = ConfigurationManager.AppSettings["AWSRegion"];
            config.UseAccelerateEndpoint = _isS3TransferAccelerationActived;
        }

        public BAmazonS3(string pAwsAccessKey, string pAwsSecretAccessKey, string pBucketname, string pKeyName = null, string pRegion = null)
        {
            _awsAccessKey = pAwsAccessKey;
            _awsSecretAccessKey = pAwsSecretAccessKey;
            this.BucketName = pBucketname;
            this.KeyName = pKeyName;
            _s3TransferAccelerationEndPoint = _s3TransferAccelerationEndPoint.Replace("{bucketName}", this.BucketName);
            AmazonS3Config config = new AmazonS3Config();
            config.ServiceURL = pRegion ?? ConfigurationManager.AppSettings["AWSRegion"];
            config.UseAccelerateEndpoint = _isS3TransferAccelerationActived;
        }

        public void SaveObject(string filePath, string keyname)
        {  
            using (var client = new AmazonS3Client(_awsAccessKey, _awsSecretAccessKey))
            {
                PutObjectRequest request = new PutObjectRequest();
                request.BucketName = this.BucketName;
                request.Key = keyname.ToFullS3KeyName(this.KeyName);
                request.FilePath = filePath;
                client.PutObject(request);
            } 
        }


        public void SaveObject(string filePath, string keyname, out ErrorSaveObjectResult result)
        {
            result = null;

            try
            {

                using (var client = new AmazonS3Client(_awsAccessKey, _awsSecretAccessKey))
                {
                    PutObjectRequest request = new PutObjectRequest();
                    request.BucketName = this.BucketName;
                    request.Key = keyname.ToFullS3KeyName(this.KeyName);
                    request.FilePath = filePath;
                    client.PutObject(request);
                }

            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                result = new ErrorSaveObjectResult($"Amazon Exception {amazonS3Exception.Message}", amazonS3Exception, filePath, keyname);
            }
            catch (Exception exception)
            {
                result = new ErrorSaveObjectResult($"SaveObject Exception {exception.Message}", exception, filePath, keyname);
            }

        }

        public void SaveObject(Stream pObject, string keyname, out ErrorSaveObjectResult result)
        {
            result = null;

            try
            {
                using (var client = new AmazonS3Client(_awsAccessKey, _awsSecretAccessKey))
                {
                    PutObjectRequest request = new PutObjectRequest();
                    request.BucketName = this.BucketName;
                    request.Key = keyname.ToFullS3KeyName(this.KeyName);
                    request.InputStream = pObject;
                    client.PutObject(request);
                }

            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                result = new ErrorSaveObjectResult($"Amazon Exception {amazonS3Exception.Message}", amazonS3Exception, pObject, keyname);
            }
            catch (Exception exception)
            {
                result = new ErrorSaveObjectResult($"SaveObject Exception {exception.Message}", exception, pObject, keyname);
            }
        }

        public void DeleteObject(string keyname)
        {
            try
            {
                DeleteObjectRequest request = new DeleteObjectRequest();
                request.BucketName = this.BucketName;
                request.Key = keyname.ToFullS3KeyName(this.KeyName);

                using (var client = new AmazonS3Client(_awsAccessKey, _awsSecretAccessKey))
                {
                    client.DeleteObject(request);
                }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                throw new Exception($"Amazon Exception {amazonS3Exception}");
            }
        }

        public void CreateKeyName(string keyname)
        {
            try
            {
                using (var client = new AmazonS3Client(_awsAccessKey, _awsSecretAccessKey))
                {
                    S3DirectoryInfo directory = new S3DirectoryInfo(client, this.BucketName, keyname.ToFullS3KeyName(this.KeyName));
                    directory.Create();
                }

            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                throw new Exception($"Amazon Exception {amazonS3Exception}");
            }
        }



    }


}
