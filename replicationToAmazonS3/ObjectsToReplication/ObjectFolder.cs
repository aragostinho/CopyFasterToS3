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
    public class ObjectFolder : AbstractInterpreter
    {

        string fromFolder = ConfigurationManager.AppSettings["rootFolderName"];
        string toBucket = ConfigurationManager.AppSettings["bucketName"];

        public override string Description()
        {
            return string.Format("Copy {0} to {1}", fromFolder, toBucket);
        }
        public override void Execute(string[] args)
        {

            try
            {
                Console.WriteLine("Iniciando replicação para o S3");

                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();


                ReplicationToS3.ReplicationFiles(fromFolder, toBucket);

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
