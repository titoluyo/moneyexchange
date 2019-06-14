using System;
using System.Collections.Generic;

namespace Api.Fixer.Entities
{
    public class Exchange
    {
        public Guid Id { get; set; }
        public string Base { get; set; }
        public string Date { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
    }
}
