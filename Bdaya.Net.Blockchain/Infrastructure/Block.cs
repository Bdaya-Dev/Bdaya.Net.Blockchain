using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Bdaya.Net.Blockchain.Infrastructure
{
    public class Block<TData> where TData : class
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public bool Status { get; set; }
        public ObjectId BlockchainId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string PreviousHash { get; set; }
        public string Hash { get; set; }
        public string NextHash { get; set; }
        public string User { get; set; }
        public TData Data { get; set; }

        public Block(DateTime timeStamp, string previousHash, TData data , bool status  ,string userClaim)
        {
            BlockchainId = ObjectId.GenerateNewId();
            Status = status;
            User = userClaim;
            TimeStamp = timeStamp;
            PreviousHash = previousHash;
            Data = data;
            Hash = CalculatHash();
        }

        public string CalculatHash()
        {
            SHA256 sHA = SHA256.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes($"{TimeStamp}-{PreviousHash ?? "" }-{Data.ToJson()}");
            byte[] outputBytes = sHA.ComputeHash(inputBytes);
            return Convert.ToBase64String(outputBytes);
        }
    }
}
