using System;
using CoreNG.Common.Entities;
using Xunit;

namespace TestCoreNg.Common
{
    public class ScheduledTransactionShould
    {
        [Fact]
        public void HaveAKnownDefaultState()
        {
            var l = new ScheduledTransaction();
            Assert.NotNull(l);
            Assert.Equal(0, l.ScheduledTransactionId);
            Assert.True(string.IsNullOrEmpty(l.Name));
            Assert.Equal(0, l.Amount);
            Assert.Equal(ScheduleType.NotSet, l.ScheduleType);
            Assert.Equal(DateTime.MinValue,l.StartDate);
            Assert.False(l.EndDate.HasValue);
            Assert.Null(l.EndDate);

        }
    }
}