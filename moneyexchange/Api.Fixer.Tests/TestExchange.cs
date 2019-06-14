using Api.Fixer;
using Api.Fixer.Domain;
using Api.Fixer.Domain.Repositories;
using Api.Fixer.Entities;
using Api.Fixer.Services;
using Api.Fixer.Data;
using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Api.Fixer.Data.Repositories;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Moq;

namespace Api.Fixer.Tests
{
    public class TestExchange
    {
        private readonly Mock<ILogger<ExchangeRepository>> _mockLogger;

        public TestExchange()
        {
            _mockLogger = new Mock<ILogger<ExchangeRepository>>();
        }

        [Fact]
        public async void Add_WhenHaveNoEmail()
        {
            IExchangeRepository repository = GetInMemoryExchangeRepository();
            string[] targets = new string[] { "EUR" };
            Exchange exchange = await repository.GetLatestAsync("USD", targets);

            Assert.True(exchange.Rates.ContainsKey("EUR"));
            Assert.NotNull(exchange);
        }

        private IExchangeRepository GetInMemoryExchangeRepository()
        {
            
            DbContextOptions<AppDbContext> options;
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseInMemoryDatabase();
            options = builder.Options;
            AppDbContext appDbContext = new AppDbContext(options);
            appDbContext.Database.EnsureDeleted();
            appDbContext.Database.EnsureCreated();
            return new ExchangeRepository(_mockLogger.Object, appDbContext);
        }
    }
}
