using System;
using System.Collections.Generic;
using System.Text;
using FacilityManagement.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FacilityManagement.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<InventoryObject> InventoryObjects { get; set; }
        public DbSet<InventoryObjectType> InventoryObjectTypes { get; set; }
        public DbSet<InventoryObjectSystem> InventoryObjectSystems { get; set; }
        public DbSet<InventoryObjectPart> InventoryObjectParts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<InventoryObject>()
                .HasMany(c => c.Types)
                .WithOne(cst => cst.InventoryObject)
                .HasForeignKey(cst => cst.InventoryObjectId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<InventoryObjectType>()
                .HasMany(cst => cst.Systems)
                .WithOne(cs => cs.Type)
                .HasForeignKey(cs => cs.InventoryObjectTypeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<InventoryObjectSystem>()
                .HasMany(cs => cs.Parts)
                .WithOne(p => p.Systems)
                .HasForeignKey(p => p.InventoryObjectSystemId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
