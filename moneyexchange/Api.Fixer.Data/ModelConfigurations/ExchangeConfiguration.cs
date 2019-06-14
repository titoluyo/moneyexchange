using Api.Fixer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Fixer.Data.ModelConfigurations
{
    public class ExchangeConfiguration : IEntityTypeConfiguration<Exchange>
    {
        public void Configure(EntityTypeBuilder<Exchange> builder)
        {
            builder.ToTable("Exchange");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(e => e.Base).IsRequired().HasMaxLength(3);
            builder.Property(e => e.Date).IsRequired().HasMaxLength(10);
            builder.Property(e => e.Rates).IsRequired().HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<Dictionary<string, decimal>>(v)
                );

            // sample data, for DEMO purposes
            var data = new List<Exchange>() {
            new Exchange { Id = Guid.NewGuid(), Base = "USD", Date = "2019-06-14", Rates = new Dictionary<string, decimal>() { { "EUR", 0.87912M }, { "JPY", 108.2412M }, { "PEN", 3.3457M } } },
            new Exchange { Id = Guid.NewGuid(), Base = "USD", Date = "2019-06-13", Rates = new Dictionary<string, decimal>() { { "EUR", 0.87924M }, { "JPY", 108.2748M }, { "PEN", 3.3434M } } },
            new Exchange { Id = Guid.NewGuid(), Base = "USD", Date = "2019-06-12", Rates = new Dictionary<string, decimal>() { { "EUR", 0.86450M }, { "JPY", 108.3457M }, { "PEN", 3.3342M } } },
            new Exchange { Id = Guid.NewGuid(), Base = "USD", Date = "2019-06-11", Rates = new Dictionary<string, decimal>() { { "EUR", 0.85014M }, { "JPY", 108.4572M }, { "PEN", 3.3321M } } },
            new Exchange { Id = Guid.NewGuid(), Base = "USD", Date = "2019-06-10", Rates = new Dictionary<string, decimal>() { { "EUR", 0.84872M }, { "JPY", 108.4785M }, { "PEN", 3.3245M } } },
            };

            builder.HasData(data);
        }
    }
}
