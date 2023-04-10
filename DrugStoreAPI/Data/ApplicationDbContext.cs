using DrugStoreAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace DrugStoreAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Drug> Drugs { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<DrugsComponents> DrugsComponents { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrdersDrugs> OrdersDrugs { get; set; }
        public DbSet<Client> Clients { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DrugConfiguration());
            modelBuilder.ApplyConfiguration(new ComponentConfiguration());
            modelBuilder.ApplyConfiguration(new DrugsComponnetsConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrdersDrugsConfiguration());
            modelBuilder.ApplyConfiguration(new ClientConfiguration());
        }
    }
}
