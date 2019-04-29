using replicationToAmazonS3.ObjectsToReplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace replicationToAmazonS3
{
    public class Program
    { 

        public static Dictionary<int, AbstractInterpreter> _ActionList;
        static void Main(string[] args)
        {

            int _intOption = 1;
            _ActionList = new Dictionary<int, AbstractInterpreter>();
            _ActionList.Add(1, ReplicationFactory.ParallelTransfer);
            _ActionList.Add(2, ReplicationFactory.NonParallelTransfer);


            try
            {
                string option = ShowOptions(_ActionList, args);
                while (int.TryParse(option, out _intOption) == false && !_ActionList.ContainsKey(_intOption))
                {
                    Console.Clear();
                    Console.WriteLine("Enter with a number code");
                    option = ShowOptions(_ActionList, args);
                }

                _ActionList[_intOption].Execute(args);

                if (args.Length != 0)
                    return; 

            }
            catch (Exception oException)
            {
                Utils.CatchErrorLog(oException);
                Console.WriteLine("Error: Invalid Key");
                Console.ReadKey();
            }

        }


        private static string ShowOptions(Dictionary<int, AbstractInterpreter> _ActionList, string[] args)
        {
            Console.WriteLine("Choose one of the options below: ");
            foreach (var item in _ActionList)
                Console.WriteLine("{0}) {1}", item.Key, item.Value.Description());

            var hasArguments = args.Length != 0;

            return hasArguments ? args[0] : Console.ReadLine();
        }




    }



}
