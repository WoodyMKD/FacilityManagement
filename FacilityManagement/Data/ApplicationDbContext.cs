using System;
using System.Collections.Generic;
using System.Text;
using FacilityManagement.Web.Areas.Identity;
using FacilityManagement.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FacilityManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<EmployeeUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<InventoryObject> InventoryObjects { get; set; }
    }
}
