using System;
using System.Collections.Generic;
using System.Text;

namespace Bdaya.Net.Blockchain.Infrastructure
{
    public class LastChanges<TData> where TData: class
    {
        public Dictionary<string,ChangeViewModel> Changes { get; set; }
        public TData Data { get; set; }
    }
}
