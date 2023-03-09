using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace DrugStore.Entities
{
    public class Drug
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public double Price { get; set; }
        public Technology? Technology { get; set; }
        public DrugStorage DrugStorage { get; set; }
        public IList<DrugsComponents> DrugsComponents { get; set; } = new List<DrugsComponents>();
    }

    public class DrugConfiguration : IEntityTypeConfiguration<Drug>
    {
        public void Configure(EntityTypeBuilder<Drug> builder)
        {
            builder
                .HasKey(d => d.Id);
            builder
                .Property(d => d.Name)
                .IsRequired(true);
            builder
                .Property(d => d.Type)
                .IsRequired(true);
            builder
                .Property(d => d.Price)
                .IsRequired(true);
            builder
                .HasOne(t => t.Technology)
                .WithOne(t => t.Drug)
                .HasForeignKey<Technology>(t => t.DrugId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true); ;
               
        }
    }
}
