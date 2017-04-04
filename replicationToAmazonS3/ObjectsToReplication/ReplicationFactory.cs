using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace replicationToAmazonS3.ObjectsToReplication
{
    public static class ReplicationFactory
    {

        private static ObjectFolder oObjectFolder;
        public static ObjectFolder ObjectFolderToS3
        {
            get
            {
                return oObjectFolder ?? (oObjectFolder = new ObjectFolder());
            }
        }

       
    }
}
