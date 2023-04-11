using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DrugStoreAPI.Entities
{
    public class Component
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
        public int CriticalAmount { get; set; }
        public virtual ICollection<DrugsComponents> DrugsComponents { get; set; } = new List<DrugsComponents>();
    }

    public class ComponentConfiguration : IEntityTypeConfiguration<Component>
    {
        public void Configure(EntityTypeBuilder<Component> builder)
        {
            builder
                .ToTable("components");
            builder
                .HasKey(d => d.Id);
            builder
                .Property(d => d.Id)
                .HasColumnName("id");
            builder
                .Property(d => d.Name)
                .HasColumnName("name")
                .IsRequired(true);
            builder
                .Property(d => d.Type)
                .HasColumnName("type")
                .IsRequired(true);
            builder
                .Property(d => d.Price)
                .HasColumnName("price")
                .IsRequired(true);
            builder
                .Property(c => c.Amount)
                .HasColumnName("amount")
                .IsRequired(true);
            builder
                .Property(c => c.CriticalAmount)
                .HasColumnName("critical_amount")
                .IsRequired(true);
        }
    }
}
