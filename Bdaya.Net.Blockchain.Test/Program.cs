using Bdaya.Net.Blockchain.Configurations;
using Bdaya.Net.Blockchain.Repositories;
using MongoDB.Bson;
using System;
using System.Threading.Tasks;

namespace Bdaya.Net.Blockchain.Test
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime?  StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
    class Program
    {
        static async Task Main(string[] args)
        {
            var configurations = new MongoConfigurations
            {
                ConnectionString = "mongodb+srv://AhmedKhalil777:Password123@db1.lbuwf.mongodb.net/ProjectsDb?retryWrites=true&w=majority",
                Database = "ProjectsDb",
                BlockCollection = "Projects"
            };
            var blockchain = new BlockchainUnitOfWork<Project>(configurations);
            // var block = await blockchain.CreateGenesisBlockAsync("Ahmed");
            //var block = await blockchain.CreateTransactionAsync("6000e4d3ae71bd225f7951c4", "Mohammad", true, new Project
            //{
            //    Id = 2,
            //    Name = "Khdsfdg",
            //    StartDate = null,
            //    EndDate = null,
            //});

            Console.WriteLine((await blockchain.GetChangesOfChain("6000e4d3ae71bd225f7951c4")).ToJson());
        }
    }
}
