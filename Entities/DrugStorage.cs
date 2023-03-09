using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DrugStore.Entities
{
    public class DrugStorage
    {
        public int Id { get; set; }
        public int DrugId { get; set; }
        public Drug Drug { get; set; }
        public int Amount { get; set; }
        public int CriticalAmount { get; set; }
    }

    public class DrugStorageConfiguration : IEntityTypeConfiguration<DrugStorage>
    {
        public void Configure(EntityTypeBuilder<DrugStorage> builder)
        {
            builder
                .HasKey(ds => ds.Id);
            builder
                .Property(ds => ds.Amount)
                .IsRequired(true);
            builder
                .Property<int>(ds => ds.CriticalAmount)
                .IsRequired(true);
            builder
                .HasOne(ds => ds.Drug)
                .WithOne(d => d.DrugStorage)
                .HasForeignKey<DrugStorage>(ds => ds.DrugId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);
        }
    }
}
