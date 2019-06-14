using Api.Fixer.Domain.Repositories;
using Api.Fixer.Entities;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Api.Fixer.Data.Repositories
{
    public class ExchangeRepository : BaseRepository, IExchangeRepository
    {
        public ExchangeRepository(AppDbContext context) : base(context) { }

        public async Task<Exchange> GetLatestAsync(string source, string[] targets)
        {
            Exchange result = null;
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
            return result;
        }

        public async Task<List<Exchange>> GetAllAsync()
        {
            List<Exchange> result = null;
            result = await _context.Exchanges.ToListAsync();
            return result;
        }

    }
}
