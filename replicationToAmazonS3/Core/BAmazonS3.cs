using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace replicationToAmazonS3.Core
{
    public class BAmazonS3 : IBAmazonS3
    {
        private string _awsAccessKey;
        private string _awsSecretAccessKey;
        private string _bucketname;
        private AmazonS3Config config;


        public BAmazonS3()
        {
            _awsAccessKey = ConfigurationManager.AppSettings["AWSAccessKey"];
            _awsSecretAccessKey = ConfigurationManager.AppSettings["AWSSecretKey"];
            _bucketname = ConfigurationManager.AppSettings["bucketname"];
            config = new AmazonS3Config();
            config.ServiceURL = ConfigurationManager.AppSettings["AWSRegion"];
        }

        public BAmazonS3(string pawsAccessKey, string pawsSecretAccessKey, string pbucketname, string pRegion = null)
        {
            _awsAccessKey = pawsAccessKey;
            _awsSecretAccessKey = pawsSecretAccessKey;
            _bucketname = pbucketname;
            AmazonS3Config config = new AmazonS3Config();
            config.ServiceURL = pRegion ?? ConfigurationManager.AppSettings["AWSRegion"];
        }

        public string BucketName
        {
            get { return _bucketname; }
        }



        public void SaveObject(string pFilePath, string keyname)
        {
            try
            {
                using (var client = Amazon.AWSClientFactory.CreateAmazonS3Client(_awsAccessKey, _awsSecretAccessKey))
                {
                    PutObjectRequest request = new PutObjectRequest();
                    request.BucketName = _bucketname;
                    request.Key = keyname;
                    request.FilePath = pFilePath;
                    using (var response = client.PutObject(request)) { };
                }

            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    throw new Exception("Please check the provided AWS Credentials. If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
                }
                throw new Exception(string.Format("An error occurred with the message '{0}' when writing an object", amazonS3Exception.Message));
            }
        }

        public void SaveObject(Stream pObject, string keyname)
        {
            try
            {
                using (var client = Amazon.AWSClientFactory.CreateAmazonS3Client(_awsAccessKey, _awsSecretAccessKey))
                {
                    PutObjectRequest request = new PutObjectRequest();
                    request.BucketName = _bucketname;
                    request.Key = keyname;
                    request.InputStream = pObject;
                    using (var response = client.PutObject(request)) { };

                }

            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    throw new Exception("Please check the provided AWS Credentials. If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
                }
                throw new Exception(string.Format("An error occurred with the message '{0}' when writing an object", amazonS3Exception.Message));
            }
        }
        public void DeleteObject(string keyname)
        {
            try
            {
                DeleteObjectRequest request = new DeleteObjectRequest();
                request.BucketName = _bucketname;
                request.Key = keyname;

                using (var client = Amazon.AWSClientFactory.CreateAmazonS3Client(_awsAccessKey, _awsSecretAccessKey))
                {
                    client.DeleteObject(request);
                }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    throw new Exception("Please check the provided AWS Credentials. If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
                }
                throw new Exception(string.Format("An error occurred with the message '{0}' when deleting an object", amazonS3Exception.Message));
            }
        }




    }


}
