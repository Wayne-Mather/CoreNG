using System;
using CoreNG.Common.Entities;
using Xunit;

namespace TestCoreNg.Common
{
    public class CategoryShould
    {
        [Fact]
        public void HaveAKnownDefaultState()
        {
            var l = new Category();
            Assert.NotNull(l);
            Assert.Equal(0, l.CategoryId);
            Assert.True(string.IsNullOrEmpty(l.Name));
            Assert.True(string.IsNullOrEmpty(l.Comments));
            Assert.Null(l.Transactions);
        }
    }
}