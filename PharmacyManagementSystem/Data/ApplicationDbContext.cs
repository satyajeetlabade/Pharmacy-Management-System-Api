using Microsoft.EntityFrameworkCore;
using PharmacyManagementSystem.Models;

namespace PharmacyManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options ) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Drug> Drugs { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Inventory> Inventories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Enum Conversions
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();

            modelBuilder.Entity<Drug>()
                .Property(d => d.DrugType)
                .HasConversion<string>();

            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<string>();

            //Relationships
           modelBuilder.Entity<Order>()
                .HasOne(o => o.Doctor)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(oi => oi.VerifiedBy)
                .WithMany()
                .HasForeignKey(o => o.VerifiedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
               .HasOne(oi => oi.Order)
               .WithMany(o => o.OrderItems)
               .HasForeignKey(oi => oi.OrderId)
               .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Drug)
                .WithMany(d => d.OrderItems)
                .HasForeignKey(oi => oi.DrugId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
