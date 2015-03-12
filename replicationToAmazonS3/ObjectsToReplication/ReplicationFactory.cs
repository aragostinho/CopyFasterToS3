using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace replicationToAmazonS3.ObjectsToReplication
{
    public static class ReplicationFactory
    {

        private static CompanyFolder oCompanyFolder;
        public static CompanyFolder CompanyFolderToS3
        {
            get
            {
                return oCompanyFolder ?? (oCompanyFolder = new CompanyFolder());
            }
        }

       
    }
}
