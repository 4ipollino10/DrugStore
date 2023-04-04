using DrugStoreAPI.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DrugStoreAPI.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime AppointedDate { get; set; }
        public DateTime ReceivingDate { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public int ClientId { get; set; }
        public Client? Client { get; set; }
        public ICollection<OrdersDrugs> OrdersDrugs { get; set; } = new List<OrdersDrugs>();
    }

    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder
                .ToTable("orders");
            builder
                .HasKey(o => o.Id);
            builder
                .Property(o => o.Id)
                .HasColumnName("id");
            builder
                .Property(o => o.OrderDate)
                .HasColumnName("order_date")
                .IsRequired(true);
            builder
                .Property(o => o.ClientId)
                .HasColumnName("client_id")
                .IsRequired(true);
            builder
                .Property(o => o.OrderStatus)
                .HasColumnName("status")
                .IsRequired(true);
            builder
                .Property(o => o.AppointedDate)
                .HasColumnName("appointed_date")
                .IsRequired(true);
            builder
                .Property(o => o.ReceivingDate)
                .HasColumnName("receiving_date")
                .IsRequired(true);
            builder
                .HasOne(o => o.Client)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.ClientId);
        }
    }
}
