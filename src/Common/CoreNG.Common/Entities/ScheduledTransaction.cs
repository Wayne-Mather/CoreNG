using System;

namespace CoreNG.Common.Entities
{
    public enum ScheduleType
    {
        NotSet = 0,
        Daily,
        Weekly,
        Fortnightly,
        Monthly,
        Quarterly,
        HalfYearly,
        Yearly,
        EveryOtherMonth,
        EveryOtherQuarter,
        EveryOtherYear
    }
    
    public class ScheduledTransaction
    {
        public int ScheduledTransactionId { get; set; }
        public int AccountId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public ScheduleType ScheduleType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        
        public decimal Amount { get; set; }
        
        public Byte[] Timestamp { get; set; }
        
        public Account Account { get; set; }
        public Category Category { get; set; }
    }
}