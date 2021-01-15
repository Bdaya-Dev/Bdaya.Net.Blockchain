using Bdaya.Net.Blockchain.Infrastructure;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bdaya.Net.Blockchain.Repositories
{
    public interface IBlockchainUnitOfWork<TData> where TData : class , new()
    {
        Task<Block<TData>> CreateGenesisBlockAsync(string userClaim);
        FilterDefinition<Block<TData>> GetFilterOfLastBlock(string blockchainId);
        Task<Block<TData>> GetLastBlockAsync(string blockchainId);
        Task<Block<TData>> GetLastBlockWithoutVerifyingAsync(string blockchainId);
        Task<Block<TData>> CreateTransactionAsync(string blockchainId, string userClaim, bool status, TData data);
        Task<IEnumerable<Block<TData>>> GetAscendingBlockchainAsync(string blockchainId);
        Task<IEnumerable<Block<TData>>> GetDecendingBlockchainAsync(string blockchainId);
        Task<TData> GetDataOfChain(string blockchainId);
        Task<LastChanges<TData>> GetChangesOfChain(string blockchainId);

    }
}
