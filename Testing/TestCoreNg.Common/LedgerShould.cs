using System;
using CoreNG.Common.Entities;
using Xunit;

namespace TestCoreNg.Common
{
    public class LedgerShould
    {
        [Fact]
        public void HaveAKnownDefaultState()
        {
            var l = new Ledger();
            Assert.NotNull(l);
            Assert.Equal(0, l.AccountId);
            Assert.Equal(0M,l.Amount);
            Assert.NotEqual(Guid.Empty, l.TransactionId);
            Assert.Equal(DateTime.MinValue, l.DateTime);
        }
    }
}