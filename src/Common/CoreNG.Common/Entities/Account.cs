using System.Collections.Generic;
using System.Transactions;

namespace CoreNG.Common.Entities
{
    public class Account
    {
        public int AccountId { get; set; }
        public string Name { get; set; }
        
        public decimal Balance { get; set; }
        public string Comments { get; set; }
        
        public List<Transaction> Transactions { get; set; }
    }
}