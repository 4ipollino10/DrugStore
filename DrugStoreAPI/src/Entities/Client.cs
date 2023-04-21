using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DrugStoreAPI.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }

    public class ClientConfiguration : IEntityTypeConfiguration<Client> 
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder
                .ToTable("clients");
            builder
                .HasKey(c => c.Id);
            builder
                .Property(c => c.Id)
                .HasColumnName("id");
            builder
                .Property(c => c.Name)
                .HasColumnName("name")
                .IsRequired(true);
            builder
                .Property(c => c.PhoneNumber)
                .HasColumnName("phone_number")
                .IsRequired(true);
            builder
                .Property(c => c.Address)
                .HasColumnName("address")
                .IsRequired(true);
        }
    }
}
