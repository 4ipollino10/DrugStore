using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DrugStore.Entities
{
    public class ComponentStorage
    {
        public int Id { get; set; }
        public int ComponentId { get; set; }
        public Component Component { get; set; }
        public int Amount { get; set; }
        public int CriticalAmount { get; set; }
    }

    public class ComponentStorageConfiguration : IEntityTypeConfiguration<ComponentStorage>
    {
        public void Configure(EntityTypeBuilder<ComponentStorage> builder)
        {
            builder
                .HasKey(cs => cs.Id);
            builder
                .Property(cs => cs.Amount)
                .IsRequired(true);
            builder
                .Property(cs => cs.CriticalAmount)
                .IsRequired(true);
            builder
                .HasOne(cs => cs.Component)
                .WithOne(c => c.ComponentStorage)
                .HasForeignKey<ComponentStorage>(cs => cs.ComponentId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);
        }
    }
}
