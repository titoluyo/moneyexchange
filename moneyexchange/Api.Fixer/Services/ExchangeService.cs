using Api.Fixer.Domain.Repositories;
using Api.Fixer.Domain.Services;
using Api.Fixer.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Fixer.Services
{
    public class ExchangeService : IExchangeService
    {
        public readonly IExchangeRepository _exchangeRepository;
        private readonly ILogger _logger;

        public ExchangeService(
            IExchangeRepository exchangeRepository,
            ILogger<ExchangeService> logger
            )
        {
            _exchangeRepository = exchangeRepository;
            _logger = logger;
        }

        public async Task<Exchange> GetLatestAsync(string source, string[] targets)
        {
            Exchange result = null;
            try
            {
                result = await _exchangeRepository.GetLatestAsync(source, targets);
            }
            catch (Exception ex)
            {
                _logger.LogError(10201, ex, "Error in ExchangeService.GetLatestAsync");
            }
            return result;
        }

        public async Task<List<Exchange>> GetAllAsync()
        {
            List<Exchange> result = null;
            try
            {
                result = await _exchangeRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(10202, ex, "Error in ExchangeService.GetAllAsync");
            }
            return result;
        }
    }
}
