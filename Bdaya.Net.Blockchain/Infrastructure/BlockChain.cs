using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bdaya.Net.Blockchain.Infrastructure
{
    public class BlockChain
    {
        public string Name { get; set; }
        public Dictionary<string, ObjectId> Chains { get; set; }
    }
}
