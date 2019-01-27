using CoreNG.Common.Entities;
using CoreNG.Persistence.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreNG.Persistence.Sqlite.Configurations
{
    public class LedgerConfiguration: IEntityTypeConfiguration<Ledger>
    {
        public void Configure(EntityTypeBuilder<Ledger> builder)
        {
            builder.ToTable("Ledger");

            builder.HasKey(e => e.LedgerTransactionId).HasName("PK_Ledger");

            builder.Property(x => x.DateTime).HasColumnType(Constants.ColumnTypes.DateTime);

            builder.Property(x => x.Timestamp).ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();

            builder.HasIndex(x => x.DateTime).IsUnique(false);

            builder.HasOne(x => x.Account)
                .WithMany(x => x.Transactions)
                .HasForeignKey(x => x.AccountId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Ledger_Account");
            
            builder.HasOne(x => x.Category)
                .WithMany(x => x.Transactions)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Ledger_Category");
            
        }
    }
}