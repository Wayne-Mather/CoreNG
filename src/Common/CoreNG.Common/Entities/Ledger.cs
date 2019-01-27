using System;

namespace CoreNG.Common.Entities
{
    public class Ledger
    {
        public Ledger()
        {
            this.LedgerTransactionId = Guid.NewGuid();
        }
        
        public Guid LedgerTransactionId { get; set; }
        
        public int AccountId { get; set; }
        public int CategoryId { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Amount { get; set; }
        
        public int Colour { get; set; }
        public bool Reconciled { get; set; }
        
        public Byte[] Timestamp { get; set; }
        
        public Account Account { get; set; }
        public Category Category { get; set; }
    }
}