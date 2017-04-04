using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace replicationToAmazonS3.Core
{
    public static class ReplicationToS3
    {

        private static void ParallelRecursive(string localDir, BAmazonS3 pBAmazonS3, string cleanPath = null)
        {
            Parallel.ForEach(Directory.GetDirectories(localDir), dirPath =>
            {
                string currentFolder = Path.GetFileName(dirPath);
                string currentKey = cleanPath != null ? dirPath.Replace(cleanPath, string.Empty).Replace(@"\", "/") : dirPath.Replace(@"\", "/");

                Console.WriteLine("Copying....");
                Parallel.ForEach(Directory.GetFiles(dirPath), filePath =>
                {
                    string currentFile = Path.GetFileName(filePath);
                    pBAmazonS3.SaveObject(filePath, string.Format(@"{0}/{1}", currentKey, currentFile));
                });
                ParallelRecursive(dirPath, pBAmazonS3, cleanPath);
            });
        }
        private static void ClassicRecursive(string localDir, BAmazonS3 pBAmazonS3, string cleanPath = null)
        {
            foreach (string dirPath in Directory.GetDirectories(localDir))
            {
                string currentFolder = Path.GetFileName(dirPath);
                string currentKey = cleanPath != null ? dirPath.Replace(cleanPath, string.Empty).Replace(@"\", "/") : dirPath.Replace(@"\", "/");


                Console.WriteLine("Copying....");
                foreach (string filePath in Directory.GetFiles(dirPath))
                {
                    string currentFile = Path.GetFileName(filePath);
                    pBAmazonS3.SaveObject(filePath, string.Format(@"{0}/{1}", currentKey, currentFile));
                    Console.WriteLine(string.Format("File {0} copied", Path.GetFileName(filePath)));
                }
                ClassicRecursive(dirPath, pBAmazonS3, cleanPath);
            }
        }

        public static void ReplicationFiles(string localPath, string bucketName)
        {
            string AWSSecretKey = ConfigurationManager.AppSettings["AWSSecretKey"];
            string AWSAccessKey = ConfigurationManager.AppSettings["AWSAccessKey"];
            BAmazonS3 oBAmazonS3 = new BAmazonS3(AWSAccessKey, AWSSecretKey, bucketName);
            Console.WriteLine("Starting to copy local files from '{0}' to '{1}' bucket", localPath, bucketName);
            ParallelRecursive(localPath, oBAmazonS3, localPath);
            Console.WriteLine("***Replication finished**");
        }



    }
}
