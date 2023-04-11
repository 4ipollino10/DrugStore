using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace DrugStoreAPI.Entities
{
    public class DrugsComponents
    {
        public int DrugId { get; set; }
        public virtual Drug Drug { get; set; }
        public int ComponentId { get; set; }
        public virtual Component Component { get; set; }
        public int Amount { get; set; }
    }

    public class DrugsComponnetsConfiguration : IEntityTypeConfiguration<DrugsComponents>
    {
        public void Configure(EntityTypeBuilder<DrugsComponents> builder)
        {
            builder
                .ToTable("drugs_components");
            builder
                .HasKey(dc => new { dc.DrugId, dc.ComponentId });
            builder
                .HasOne(dc => dc.Drug)
                .WithMany(d => d.DrugsComponents)
                .HasForeignKey(dc => dc.DrugId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);
            builder
                .HasOne(dc => dc.Component)
                .WithMany(c => c.DrugsComponents)
                .HasForeignKey(dc => dc.ComponentId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);
            builder
                .Property(dc => dc.Amount)
                .HasColumnName("amount")
                .IsRequired(true);
            builder
                .Property(dc => dc.ComponentId)
                .HasColumnName("component_id");
            builder
                .Property(dc => dc.DrugId)
                .HasColumnName("drug_id");
        }
    }
}
