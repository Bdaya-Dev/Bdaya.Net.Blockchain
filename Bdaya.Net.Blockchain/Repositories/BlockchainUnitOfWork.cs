using Bdaya.Net.Blockchain.Configurations;
using Bdaya.Net.Blockchain.Infrastructure;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bdaya.Net.Blockchain.Repositories
{
    public class BlockchainUnitOfWork<TData> : IBlockchainUnitOfWork<TData> where TData : class, new()
    {
        private readonly IMongoCollection<Block<TData>> _blocks;
        public BlockchainUnitOfWork(MongoConfigurations configurations)
        {
            var client = new MongoClient(configurations.ConnectionString);
            var mongoDb = client.GetDatabase(configurations.Database);
            _blocks = mongoDb.GetCollection<Block<TData>>(configurations.BlockCollection);
        }

        public async Task<Block<TData>> CreateGenesisBlockAsync(string userClaim)
        {
            var block = new Block<TData>(DateTime.Now, null, null, true,userClaim);
            block.Id = ObjectId.GenerateNewId();
            block.BlockchainId = ObjectId.GenerateNewId();
            block.Status = true;
            await _blocks.InsertOneAsync(block);
            return block;
        }

        public FilterDefinition<Block<TData>> GetFilterOfLastBlock(string blockchainId)
        {
            return Builders<Block<TData>>.Filter.Eq(x => x.NextHash, null) &
                Builders<Block<TData>>.Filter.Eq(x => x.Status, true) &
                Builders<Block<TData>>.Filter.Eq(x => x.BlockchainId, ObjectId.Parse(blockchainId));
        }
        public async Task<Block<TData>> GetLastBlockAsync(string blockchainId)
        {
            var filter = Builders<Block<TData>>.Filter.Eq(x => x.NextHash, null) &
                Builders<Block<TData>>.Filter.Eq(x => x.Status, true) &
                Builders<Block<TData>>.Filter.Eq(x => x.BlockchainId, ObjectId.Parse(blockchainId));
            return await _blocks.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Block<TData>> GetLastBlockWithoutVerifyingAsync(string blockchainId)
        {
            var filter = Builders<Block<TData>>.Filter.Eq(x => x.NextHash, null) &
                Builders<Block<TData>>.Filter.Eq(x => x.BlockchainId, ObjectId.Parse(blockchainId));
            return await _blocks.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Block<TData>> CreateTransactionAsync(string blockchainId,string userClaim, bool status ,TData data)
        { 
            var lastBlock = await GetLastBlockAsync(blockchainId);
            var block = new Block<TData>(DateTime.Now, lastBlock.Hash,data, status, userClaim);
            var updateDefinition =  Builders<Block<TData>>.Update.Set(x => x.NextHash, block.Hash);
            block.BlockchainId = ObjectId.Parse(blockchainId);
            await _blocks.UpdateOneAsync(GetFilterOfLastBlock(blockchainId), updateDefinition);
            await _blocks.InsertOneAsync(block);
            return block;
        }
        public async Task<IEnumerable<Block<TData>>> GetAscendingBlockchainAsync(string blockchainId)
        {
            var sortDefinition = Builders<Block<TData>>.Sort.Ascending(x => x.TimeStamp);
            var filter = Builders<Block<TData>>.Filter.Eq(x => x.Status, true) & Builders<Block<TData>>.Filter.Eq(x => x.BlockchainId, ObjectId.Parse(blockchainId));
            return await _blocks.Find(filter).Sort(sortDefinition).ToListAsync();
        }

        public async Task<IEnumerable<Block<TData>>> GetDecendingBlockchainAsync(string blockchainId)
        {
            var sortDefinition = Builders<Block<TData>>.Sort.Descending(x => x.TimeStamp);
            var filter = Builders<Block<TData>>.Filter.Eq(x => x.Status, true) & Builders<Block<TData>>.Filter.Eq(x => x.BlockchainId, ObjectId.Parse(blockchainId));
            return await _blocks.Find(filter).Sort(sortDefinition).ToListAsync();
        }

        public async Task<TData> GetDataOfChain(string blockchainId)
        {
            var data = new TData();
            var chain = await GetDecendingBlockchainAsync(blockchainId);
            var properties = typeof(TData).GetProperties();
            foreach (var property in properties)
            {
                foreach (var block in chain)
                {
                    var value = typeof(TData).GetProperty(property.Name).GetValue(block.Data);
                    if (value != null)
                    {
                        typeof(TData).GetProperty(property.Name).SetValue(data, value);
                        break;
                    }
                }
            }
            return data;

        }

        public async Task<LastChanges<TData>> GetChangesOfChain(string blockchainId)
        {
            var changes = new LastChanges<TData> {
                Changes = new Dictionary<string, ChangeViewModel>(),
                Data = new TData(),
             };
            var chain = await GetDecendingBlockchainAsync(blockchainId);
            var properties = typeof(TData).GetProperties();
            foreach (var property in properties)
            {
                foreach (var block in chain)
                {
                    var value = typeof(TData).GetProperty(property.Name).GetValue(block.Data);
                    if (value != null)
                    {
                        changes.Changes.Add(property.Name, new ChangeViewModel { IssueDate = block.TimeStamp, User = block.User });
                        typeof(TData).GetProperty(property.Name).SetValue(changes.Data, value);
                        break;
                    }
                }
            }
            return changes;

        }

    }
}
