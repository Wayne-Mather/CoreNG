using CoreNG.Common.Entities;
using CoreNG.Persistence.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreNG.Persistence.Sqlite.Configurations
{
    public class CategoryConfiguration: IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");

            builder.HasKey(e => e.CategoryId).HasName("PK_Category");

            builder.Property(x => x.Name).HasColumnType(Constants.ColumnTypes.CategoryName);

            builder.Property(x => x.Comments).HasColumnType(Constants.ColumnTypes.Comments);

            builder.Property(x => x.Timestamp).ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();

            builder.HasIndex(x => x.Name).IsUnique();

        }
    }
}