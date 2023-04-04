﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DrugStoreAPI.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public ICollection<Order> Orders { get; set; }
    }

    public class ClientConfiguration : IEntityTypeConfiguration<Client> 
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder
                .ToTable("clinets");
            builder
                .HasKey(c => c.Id);
            builder
                .Property(c => c.Name)
                .HasColumnName("name")
                .IsRequired(true);
            builder
                .Property(c => c.PhoneNumber)
                .HasColumnName("phone_number")
                .IsRequired(true);
        }
    }
}