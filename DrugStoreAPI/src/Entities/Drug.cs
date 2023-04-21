using DrugStoreAPI.src.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DrugStoreAPI.Entities
{
    public class Drug
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public MedicamentType Type { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
        public int CriticalAmount { get; set; }
        public string Technology { get; set; }
        public double CookingTime { get; set; }
        public virtual ICollection<DrugsComponents> DrugsComponents { get; set; } = new List<DrugsComponents>();
        public virtual ICollection<OrdersDrugs> OrdersDrugs { get; set; } = new List<OrdersDrugs>();
    }

    public class DrugConfiguration : IEntityTypeConfiguration<Drug>
    {
        public void Configure(EntityTypeBuilder<Drug> builder)
        {
            builder
                .ToTable("drugs");
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
                .Property(d => d.Technology)
                .HasColumnName("technology")
                .IsRequired(true);
            builder
                .Property(d => d.Type)
                .HasColumnName ("type")
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
            builder
                .Property(d => d.CookingTime)
                .HasColumnName("cooking_time")
                .IsRequired(true);
        }
    }
}
