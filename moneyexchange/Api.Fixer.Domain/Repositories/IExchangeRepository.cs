using Api.Fixer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Api.Fixer.Domain.Repositories
{
    public interface IExchangeRepository
    {
        Task<Exchange> GetLatestAsync(string source, string[] targets);
        Task<List<Exchange>> GetAllAsync();
    }
}
