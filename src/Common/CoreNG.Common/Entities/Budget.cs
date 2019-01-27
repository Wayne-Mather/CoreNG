using System;
using System.Reflection.Metadata.Ecma335;

namespace CoreNG.Common.Entities
{
    public class Budget
    {
        public int BudgetId { get; set; }
        
        public int Year { get; set; }
        public int Month { get; set; }
        
        public int BudgetCategoryId { get; set; }
        
        public decimal Budgeted { get; set; }
        public decimal TotalTransacted { get; set; }
        
        public Byte[] Timestamp { get; set; }
    }
}