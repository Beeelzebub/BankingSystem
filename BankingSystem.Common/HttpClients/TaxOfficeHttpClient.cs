using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Common.HttpClients
{
    public class TaxOfficeHttpClient : HttpClientBase
    {
        private const string TaxOfficeRoute = "api/TransferTax";

        public TaxOfficeHttpClient(HttpClient httpClient, IConfiguration configuration)
            : base(httpClient, configuration, configuration["TaxOfficeUrl"])
        {

        }

        public async Task<float> GetTransferTaxAsync(float transferAmount)
        {
            return await GetAsync<float>($"{TaxOfficeRoute}/{transferAmount}");
        }
    }
}
