using CoreNG.Common.Entities;
using Xunit;

namespace TestCoreNg.Common
{
    public class AccountShould
    {
        [Fact]
        public void HaveAKnownDefaultState()
        {
            var acc = new Account();
            Assert.Equal(0, acc.AccountId);
            Assert.Equal(0.00M, acc.Balance);
            Assert.True(string.IsNullOrEmpty(acc.Name));
            Assert.True(string.IsNullOrEmpty(acc.Comments));
        }
    }
}