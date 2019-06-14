using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api.Fixer.Entities;
using Api.Fixer.Domain.Services;
using Microsoft.Extensions.Logging;

namespace Api.Fixer
{
    [Route("/")]
    [ApiController]
    public class ExchangeController : ControllerBase
    {
        private readonly IExchangeService _exchangeService;
        private readonly ILogger _logger;

        public ExchangeController(
            IExchangeService exchangeService,
            ILogger<ExchangeController> logger
            )
        {
            _exchangeService = exchangeService;
            _logger = logger;
        }

        // GET latest
        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestAsync(
            [FromQuery(Name = "base")]string source = "USD",
            [FromQuery(Name = "symbols")]string symbols = "EUR"
            )
        {
            Exchange result = null;
            _logger.LogDebug($"ExchangeController.GetLatestAsync - source: {source}, symbols: {symbols}");
            try
            {
                var targets = symbols.Split(',');
                result = await _exchangeService.GetLatestAsync(source, targets);
            }
            catch (Exception ex)
            {
                _logger.LogError(10001, ex, "Error in controller");
            }
            return Ok(result);
        }

        /* for local dev only
        [HttpGet]
        public async Task<List<Exchange>> GetAll()
        {
            return await _exchangeService.GetAllAsync();
        }
        // */
    }
}
