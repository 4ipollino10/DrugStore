using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace DrugStore.Entities
{
    public class Method
    {
        public int Id { get; set; }
        public string? MethodName { get; set; }
        public IList<Guide> Guides { get; set; } = new List<Guide>();
    }

    public class MetohdConiguration : IEntityTypeConfiguration<Method>
    {
        public void Configure(EntityTypeBuilder<Method> builder)
        {
            builder
                .HasKey(m => m.Id);
            builder
                .Property(m => m.MethodName)
                .IsRequired(true);
        }
    }
}
