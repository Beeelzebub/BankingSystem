using BankingSystem.Common.DTO;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Text;
using System.Text.Json;

namespace BankingSystem.Common.HttpClients
{
    public class JuridicalRegisterHttpClient : HttpClientBase
    {
        private const string JuridicalRegisterRoute = "api/JuridicalRegister";

        public JuridicalRegisterHttpClient(HttpClient httpClient, IConfiguration configuration)
            : base(httpClient, configuration, configuration["JuridicalRegisterUrl"])
        {

        }

        public async Task<LegalPersonPaymentInfo> GetLegalPersonInfoAsync(string bankAccountNumber)
        {
            return await GetAsync<LegalPersonPaymentInfo>($"{JuridicalRegisterRoute}/{bankAccountNumber}");
        }

        public async Task<bool> AddBankNumberToOrganisationAsync(CreateLegalPersonAccountDto data, bool isRetry = false)
        {
            if (string.IsNullOrEmpty(BearerToken))
            {
                await RefreshBearerToken();
            }

            var json = JsonSerializer.Serialize(data);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            using var response = await _httpClient.PostAsync($"{JuridicalRegisterRoute}", payload);

            if (response.StatusCode == HttpStatusCode.Unauthorized && !isRetry)
            {
                await RefreshBearerToken();
                return await AddBankNumberToOrganisationAsync(data, true);
            }

            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();

            var apiResult = await JsonSerializer.DeserializeAsync<ApiResult>(stream, _serializerOptions);

            return apiResult.Success;
        }
    }
}
