using System;
using System.Collections.Generic;
using System.Text;

namespace Bdaya.Net.Blockchain.Configurations
{
    public class MongoConfigurations
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
        public string BlockCollection { get; set; }
        public string BlockchainCollection { get; set; }
    }
}
