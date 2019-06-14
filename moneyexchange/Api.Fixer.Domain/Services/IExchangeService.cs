using Api.Fixer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Fixer.Domain.Services
{
    public interface IExchangeService
    {
        Task<Exchange> GetLatestAsync(string source, string[] targets);
        Task<List<Exchange>> GetAllAsync();
    }
}
