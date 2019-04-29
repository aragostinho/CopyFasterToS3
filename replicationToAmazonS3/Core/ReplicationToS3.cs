using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace replicationToAmazonS3.Core
{
    public static class ReplicationToS3
    {
        private static Stopwatch _stopwatch;
        private static bool _copyEmptyFolders = false;
        private static int recycleBufferIntMinutes = ConfigurationManager.AppSettings["RecycleBufferInMinutes"].ToInt();
        private static ErrorSaveObjectResult errorResult = null;

        private static void ReleaseBuffer()
        {
            var elapsedTime = _stopwatch.Elapsed.Minutes;
            if (elapsedTime >= recycleBufferIntMinutes)
            {

                Console.Clear();
                GC.Collect();

                _stopwatch = new Stopwatch();
                _stopwatch.Start();

            }
        }

        private static void ParallelRecursive(string localDir, BAmazonS3 pBAmazonS3, string cleanPath = null)
        {
            if (Directory.Exists(localDir))
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

                        pBAmazonS3.SaveObject(filePath, string.Format(@"{0}/{1}", currentKey, currentFile), out errorResult);
                        
                        ReleaseBuffer();

                        if (errorResult == null)
                            Console.WriteLine(string.Format("File {0} copied", Path.GetFileName(filePath)));
                        else
                        {
                            Console.WriteLine(string.Format("Error copying file {0}", Path.GetFileName(filePath)));
                            Utils.CatchErrorLog(errorResult, currentFile, currentKey);
                        }

                    });

                    if (_copyEmptyFolders && files.Count() == 0)
                        pBAmazonS3.CreateKeyName($@"{currentKey}");


                    ParallelRecursive(dirPath, pBAmazonS3, cleanPath);
                });
            }
        }

        public static void ClassicRecursive(string localDir, BAmazonS3 pBAmazonS3, string cleanPath = null)
        {
            if (Directory.Exists(localDir))
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
                          
                        pBAmazonS3.SaveObject(filePath, string.Format(@"{0}/{1}", currentKey, currentFile), out errorResult);

                        ReleaseBuffer();

                        if (errorResult == null)
                            Console.WriteLine(string.Format("File {0} copied", Path.GetFileName(filePath)));
                        else
                        {
                            Console.WriteLine(string.Format("Error copying file {0}", Path.GetFileName(filePath)));
                            Utils.CatchErrorLog(errorResult, currentFile, currentKey);
                        }


                    }

                    if (_copyEmptyFolders && files.Count() == 0)
                        pBAmazonS3.CreateKeyName($@"{currentKey}");

                    ClassicRecursive(dirPath, pBAmazonS3, cleanPath);

                }
            }
        }

        public static void ReplicationFiles(string localPath, string[] localPathList, string bucketName, string keyName, bool copyEmptyFolders, bool usingParallel = true)
        {
            string AWSSecretKey = ConfigurationManager.AppSettings["AWSSecretKey"];
            string AWSAccessKey = ConfigurationManager.AppSettings["AWSAccessKey"];

            _copyEmptyFolders = copyEmptyFolders;
            _stopwatch = new Stopwatch();
            _stopwatch.Start();

            BAmazonS3 oBAmazonS3 = new BAmazonS3(AWSAccessKey, AWSSecretKey, bucketName, keyName);
            Console.WriteLine("Starting to copy local files from '{0}' to '{1}' bucket", localPath, bucketName);

            if (localPathList == null)
            {
                if (usingParallel)
                    ParallelRecursive(localPath, oBAmazonS3, localPath);
                else
                    ClassicRecursive(localPath, oBAmazonS3, localPath);
            }
            else
            {
                Parallel.ForEach(localPathList, directoryPath =>
                {
                    if (usingParallel)
                        ParallelRecursive(directoryPath, oBAmazonS3, directoryPath.RemoveLastFolder());
                    else
                        ClassicRecursive(directoryPath, oBAmazonS3, directoryPath.RemoveLastFolder());

                });

            }
            Console.WriteLine("***Replication finished**");
        }



    }


}
