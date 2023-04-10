using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DrugStoreAPI.Entities
{
    public class OrdersDrugs
    {
        public int DrugId { get; set; }
        public Drug? Drug { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; }
    }

    public class OrdersDrugsConfiguration : IEntityTypeConfiguration<OrdersDrugs>
    {
        public void Configure(EntityTypeBuilder<OrdersDrugs> builder)
        {
            builder
                .ToTable("orders_drugs");
            builder
                .Property(d => d.DrugId)
                .HasColumnName("drug_id");
            builder
                .Property(d => d.OrderId)
                .HasColumnName("order_id");
            builder
                .HasKey(od => new { od.DrugId, od.OrderId });
            builder
                .HasOne(od => od.Drug)
                .WithMany(d => d.OrdersDrugs)
                .HasForeignKey(od => od.DrugId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);
            builder
                .HasOne(od => od.Order)
                .WithMany(o => o.OrdersDrugs)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);
        }
    }
}
