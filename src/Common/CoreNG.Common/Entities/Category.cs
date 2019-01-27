using System;
using System.Collections.Generic;
using System.Transactions;

namespace CoreNG.Common.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Comments { get; set; }
        
        public Byte[] Timestamp { get; set; }
        
        public List<Ledger> Transactions { get; set; }
        public List<ScheduledTransaction> ScheduledTransactions { get; set; }
    }
}