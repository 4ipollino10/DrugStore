using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DrugStore.Entities
{
    public class Guide
    {
        public int MethodId { get; set; }
        public Method? Method { get; set; }
        public int TechnologyId { get; set; }
        public Technology? Technology { get; set; }
    }

    public class GuideConfiguration : IEntityTypeConfiguration<Guide>
    {
        public void Configure(EntityTypeBuilder<Guide> builder)
        {
            builder
                .HasKey(g => new {g.MethodId, g.TechnologyId});
            builder
                .HasOne(g => g.Method)
                .WithMany(m => m.Guides)
                .HasForeignKey(g => g.MethodId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);
            builder
                .HasOne(g => g.Technology)
                .WithMany(t => t.Guides)
                .HasForeignKey(g => g.TechnologyId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);
        }
    }
}
