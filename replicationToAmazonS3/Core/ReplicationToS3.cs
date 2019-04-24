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

        private static bool _copyEmptyFolders = false;

        private static void ParallelRecursive(string localDir, BAmazonS3 pBAmazonS3, string cleanPath = null)
        {
            Parallel.ForEach(Directory.GetDirectories(localDir), dirPath =>
            {
                string currentFolder = Path.GetFileName(dirPath);
                string currentKey = cleanPath != null ? dirPath.Replace(cleanPath, string.Empty).Replace(@"\", "/")
                : dirPath.Replace(@"\", "/");

                var files = Directory.GetFiles(dirPath);
                Parallel.ForEach(files, filePath =>
                {
                    string currentFile = Path.GetFileName(filePath);
                    pBAmazonS3.SaveObject(filePath, string.Format(@"{0}/{1}", currentKey, currentFile));
                    Console.WriteLine(string.Format("File {0} copied", Path.GetFileName(filePath)));
                });

                if (_copyEmptyFolders && files.Count() == 0)
                    pBAmazonS3.CreateKeyName($@"{currentKey}");


                ParallelRecursive(dirPath, pBAmazonS3, cleanPath);
            });
        }
        public static void ClassicRecursive(string localDir, BAmazonS3 pBAmazonS3, string cleanPath = null)
        {
            foreach (string dirPath in Directory.GetDirectories(localDir))
            {
                string currentFolder = Path.GetFileName(dirPath);
                string currentKey = cleanPath != null ? dirPath.Replace(cleanPath, string.Empty).Replace(@"\", "/")
                    : dirPath.Replace(@"\", "/");

                var files = Directory.GetFiles(dirPath);

                foreach (string filePath in files)
                {
                    string currentFile = Path.GetFileName(filePath);
                    pBAmazonS3.SaveObject(filePath, string.Format(@"{0}/{1}", currentKey, currentFile));
                    Console.WriteLine(string.Format("File {0} copied", Path.GetFileName(filePath)));
                }

                if (_copyEmptyFolders && files.Count() == 0)
                    pBAmazonS3.CreateKeyName($@"{currentKey}");

                ClassicRecursive(dirPath, pBAmazonS3, cleanPath);
            }
        }
        public static void ReplicationFiles(string localPath, string bucketName, bool copyEmptyFolders, bool usingParallel = true)
        {
            string AWSSecretKey = ConfigurationManager.AppSettings["AWSSecretKey"];
            string AWSAccessKey = ConfigurationManager.AppSettings["AWSAccessKey"];

            _copyEmptyFolders = copyEmptyFolders;

            BAmazonS3 oBAmazonS3 = new BAmazonS3(AWSAccessKey, AWSSecretKey, bucketName);
            Console.WriteLine("Starting to copy local files from '{0}' to '{1}' bucket", localPath, bucketName);

            if (usingParallel)
                ParallelRecursive(localPath, oBAmazonS3, localPath);
            else
                ClassicRecursive(localPath, oBAmazonS3, localPath);

            Console.WriteLine("***Replication finished**");
        }



    }


}
