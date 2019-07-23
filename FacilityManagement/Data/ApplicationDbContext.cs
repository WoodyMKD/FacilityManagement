using System;
using System.Collections.Generic;
using System.Text;
using FacilityManagement.Web.Areas.Identity;
using FacilityManagement.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FacilityManagement.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<EmployeeUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<InventoryObject> InventoryObjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<InventoryObject>()
                .Property(o => o.Type)
                .HasConversion(new EnumToStringConverter<InventoryObjectType>());
        }
    }
}
