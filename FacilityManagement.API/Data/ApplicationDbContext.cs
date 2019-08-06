﻿using System;
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

        public DbSet<Compressor> Compressors { get; set; }
        public DbSet<CompressorSubType> CompressorSubTypes { get; set; }
        public DbSet<Models.CompressorSystem> CompressorSystems { get; set; }
        public DbSet<Part> Parts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Compressor>()
                .HasMany(c => c.CompressorSubTypes)
                .WithOne(cst => cst.Compressor)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CompressorSubType>()
                .HasOne(cst => cst.Compressor)
                .WithMany(c => c.CompressorSubTypes)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CompressorSystem>()
                .HasOne(cs => cs.CompressorSubType)
                .WithMany(cst => cst.CompressorSystems)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Part>()
                .HasOne(p => p.CompressorSystem)
                .WithMany(cs => cs.Parts)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
