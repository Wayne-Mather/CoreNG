using CoreNG.Common.Entities;
using CoreNG.Persistence.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreNG.Persistence.Sqlite.Configurations
{
    public class AccountConfiguration: IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Account");

            builder.HasKey(e => e.AccountId).HasName("PK_Account");

            builder.Property(x => x.Name).HasColumnType(Constants.ColumnTypes.AccountName);
            builder.Property(x => x.Balance).HasColumnType(Constants.ColumnTypes.Balance);
            builder.Property(x => x.Comments).HasColumnType(Constants.ColumnTypes.Comments);

            builder.Property(x => x.Timestamp).ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();

            builder.HasIndex(x => x.Name).IsUnique();

        }
    }
}