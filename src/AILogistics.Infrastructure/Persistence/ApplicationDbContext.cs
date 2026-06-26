using AILogistics.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILogistics.Infrastructure.Persistence
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Shipment> Shipment { get; set; }
        public DbSet<TrackingEvent> TrackingEvents { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Shipment>()
                .HasOne(s => s.Customer)
                .WithMany(c => c.Shipments)
                .HasForeignKey(s => s.CustomerId);

            modelBuilder.Entity<TrackingEvent>()
                .HasOne(t => t.Shipment)
                .WithMany(s => s.TrackingEvents)
                .HasForeignKey(t =>  t.ShipmentId);
        }
    }
}
