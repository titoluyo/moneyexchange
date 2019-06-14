using Api.Fixer.Domain.Repositories;
using Api.Fixer.Domain.Services;
using Api.Fixer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Fixer.Services
{
    public class ExchangeService : IExchangeService
    {
        public readonly IExchangeRepository _exchangeRepository;
        public ExchangeService(IExchangeRepository exchangeRepository)
        {
            _exchangeRepository = exchangeRepository;
        }

        public async Task<Exchange> GetLatestAsync(string source, string[] targets)
        {
            Exchange result = null;
            result = await _exchangeRepository.GetLatestAsync(source, targets);
            return result;
        }

        public async Task<List<Exchange>> GetAllAsync()
        {
            List<Exchange> result = null;
            result = await _exchangeRepository.GetAllAsync();
            return result;
        }
    }
}
