using Api.Fixer.Domain.Repositories;
using Api.Fixer.Entities;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Api.Fixer.Data.Repositories
{
    public class ExchangeRepository : BaseRepository, IExchangeRepository
    {
        private readonly ILogger _logger;

        public ExchangeRepository(
            ILogger<ExchangeRepository> logger,
            AppDbContext context) : base(context)
        {
            _logger = logger;
        }

        public async Task<Exchange> GetLatestAsync(string source, string[] targets)
        {
            Exchange result = null;
            try
            {
                _logger.LogDebug($"Executing ExchangeRepository.GetLatestAsync - {source}");
                result = await _context.Exchanges
                    .Where(x => x.Base == source)
                    .OrderByDescending(x => x.Date)
                    .Select(i => new Exchange
                    {
                        Id = i.Id,
                        Base = i.Base,
                        Date = i.Date,
                        Rates = i.Rates.Where(r => targets.Contains(r.Key)).ToDictionary(p => p.Key, p => p.Value)
                    })
                    .FirstAsync();
                // throw new Exception("test");
            }
            catch (Exception ex)
            {
                _logger.LogError(10301, ex, "Error in ExchangeRepository.GetLatestAsync");
            }
            return result;
        }

        public async Task<List<Exchange>> GetAllAsync()
        {
            List<Exchange> result = null;
            try
            {
                result = await _context.Exchanges.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(10302, ex, "Error in ExchangeRepository.GetAllAsync");
            }
            return result;
        }

    }
}
