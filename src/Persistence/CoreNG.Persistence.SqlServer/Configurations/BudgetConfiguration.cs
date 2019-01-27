using CoreNG.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreNG.Persistence.SqlServer.Configurations
{
    public class BudgetConfiguration: IEntityTypeConfiguration<Budget>
    {
        public void Configure(EntityTypeBuilder<Budget> builder)
        {
            builder.ToTable("Budget");

            builder.HasKey(e => e.BudgetId).HasName("PK_Budget");

            builder.Property(x => x.TotalTransacted).HasColumnType(Constants.ColumnTypes.Balance);
            
            builder.Property(x => x.Timestamp).ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();


        }
    }
}