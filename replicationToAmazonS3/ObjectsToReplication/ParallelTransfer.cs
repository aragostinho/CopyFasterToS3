using replicationToAmazonS3.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace replicationToAmazonS3.ObjectsToReplication
{
    public class ParallelTransfer : AbstractInterpreter
    {

        string fromFolder = ConfigurationManager.AppSettings["RootFolderName"];
        string bucketName = ConfigurationManager.AppSettings["BucketName"];
        string keyName    = ConfigurationManager.AppSettings["KeyName"];
        bool copyEmptyFolders = ConfigurationManager.AppSettings["CopyEmptyFolders"].ToBoolean();

        public override string Description()
        {
            return string.Format("Parallel Copy {0} to {1}", fromFolder, bucketName);
        }
        public override void Execute(string[] args)
        {

            try
            { 
                Console.WriteLine("Starting file replication to S3"); 
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                ReplicationToS3.ReplicationFiles(fromFolder, bucketName, keyName, copyEmptyFolders);
                stopWatch.Stop();
                TimeSpan pElapsedTime = stopWatch.Elapsed;
                Console.WriteLine(string.Format("Process accomplished --- Elapsed time: {0}h-{1}m-{2}s-{3}ms",
                    pElapsedTime.Hours,
                    pElapsedTime.Minutes,
                    pElapsedTime.Seconds,
                    pElapsedTime.Milliseconds)
               );

                string pressToFinish = Console.ReadLine();

            }
            catch (Exception oException)
            {
                Console.WriteLine("Erro: {0}", oException.Message);
                Console.ReadKey();
            }



        }

    }
}
