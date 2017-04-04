using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace replicationToAmazonS3.ObjectsToReplication
{
    public static class ReplicationFactory
    {

        private static ParallelTransfer _parallelTransfer;
        public static ParallelTransfer ParallelTransfer
        {
            get
            {
                return _parallelTransfer ?? (_parallelTransfer = new ParallelTransfer());
            }
        }
        private static NonParallelTransfer _nonParallelTransfer;
        public static NonParallelTransfer NonParallelTransfer
        {
            get
            {
                return _nonParallelTransfer ?? (_nonParallelTransfer = new NonParallelTransfer());
            }
        } 

    }
}
