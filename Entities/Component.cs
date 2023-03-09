using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DrugStore.Entities
{
    public class Component
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public double Price { get; set; }
        public ComponentStorage ComponentStorage { get; set; }
        public ICollection<DrugsComponents> DrugsComponents { get; set; } = new List<DrugsComponents>();
    }

    public class ComponentConfiguration : IEntityTypeConfiguration<Component>
    {
        public void Configure(EntityTypeBuilder<Component> builder)
        {
            builder
                .HasKey(c => c.Id);
            builder
                .Property(c => c.Name)
                .IsRequired(true);
            builder
                .Property(c => c.Type)
                .IsRequired(true);
            builder
                .Property(c => c.Price)
                .IsRequired(true);
        }
    }
}
