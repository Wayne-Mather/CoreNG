using CoreNG.Common.Entities;
using CoreNG.Persistence.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreNG.Persistence.Sqlite.Configurations
{
    public class ScheduledTransactionConfiguration: IEntityTypeConfiguration<ScheduledTransaction>
    {
        public void Configure(EntityTypeBuilder<ScheduledTransaction> builder)
        {
            builder.ToTable("ScheduledTransaction");

            builder.HasKey(e => e.ScheduledTransactionId).HasName("PK_ScheduledTransaction");

            builder.Property(x => x.Name).HasColumnType(Constants.ColumnTypes.ScheduledName);
            builder.Property(x => x.Amount).HasColumnType(Constants.ColumnTypes.Balance);
            
            builder.Property(x => x.StartDate).HasColumnType(Constants.ColumnTypes.DateTime);
            builder.Property(x => x.EndDate).HasColumnType(Constants.ColumnTypes.DateTime).IsRequired(false);

            builder.Property(x => x.Timestamp).ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();

            builder.HasIndex(x => x.Name).IsUnique(true);

            builder.HasOne(x => x.Account)
                .WithMany(x => x.ScheduledTransactions)
                .HasForeignKey(x => x.AccountId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_ScheduledTransaction_Account");
            
            builder.HasOne(x => x.Category)
                .WithMany(x => x.ScheduledTransactions)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_ScheduledTransaction_Category");
            
        }
    }
}