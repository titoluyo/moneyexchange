using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api.Fixer.Entities;
using Api.Fixer.Domain.Services;

namespace Api.Fixer
{
    [Route("/")]
    [ApiController]
    public class ExchangeController : ControllerBase
    {
        private readonly IExchangeService _exchangeService;

        public ExchangeController(IExchangeService exchangeService)
        {
            _exchangeService = exchangeService;
        }

        // GET latest
        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestAsync(
            [FromQuery(Name = "base")]string source = "USD",
            [FromQuery(Name = "symbols")]string symbols = "EUR"
            )
        {
            Exchange result = null;
            var targets = symbols.Split(',');
            result = await _exchangeService.GetLatestAsync(source, targets);
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
