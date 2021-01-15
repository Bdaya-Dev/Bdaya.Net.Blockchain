using System;
using System.Collections.Generic;
using System.Text;

namespace Bdaya.Net.Blockchain.Infrastructure
{
    public class CoinChain<TData>
    {
        public int Reward { get; set; }
        public TData Data { get; set; }
        public int Difficulty { get; set; }
    }
}
