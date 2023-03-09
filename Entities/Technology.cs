using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrugStore.Entities
{
    [Index(nameof(DrugId), IsUnique = true)]
    public class Technology
    {
        public int Id { get; set; }
        public string? Recipe { get; set; }
        public int DrugId { get; set; }
        public Drug Drug { get; set; }
        public IList<Guide> Guides { get; set; } = new List<Guide>();
    }

    public class TechnologyConfiguration : IEntityTypeConfiguration<Technology>
    {
        public void Configure(EntityTypeBuilder<Technology> builder)
        {
            builder
                .HasKey(t => t.Id);
            builder
                .Property(t => t.Recipe)
                .IsRequired(true);
            builder
                .HasOne(t => t.Drug)
                .WithOne(d => d.Technology)
                .HasForeignKey<Technology>(t => t.DrugId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);
        }
    }
}
