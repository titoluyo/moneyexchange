using Api.Fixer.Data.ModelConfigurations;
using Api.Fixer.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Api.Fixer.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Exchange> Exchanges { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ExchangeConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
