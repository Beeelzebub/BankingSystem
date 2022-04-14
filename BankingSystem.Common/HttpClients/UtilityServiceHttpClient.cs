using BankingSystem.Common.DTO;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Text;
using System.Text.Json;

namespace BankingSystem.Common.HttpClients
{
    public class UtilityServiceHttpClient : HttpClientBase
    {
        private const string UtilityServiceRoute = "api/TransferTax";

        public UtilityServiceHttpClient(HttpClient httpClient, IConfiguration configuration)
            : base(httpClient, configuration, configuration["UtilityServiceUrl"])
        {

        }

        public async Task<float> PayUtilityServiceAsync(string bankAccountNumber, float amount, bool isRetry = false)
        {
            if (string.IsNullOrEmpty(BearerToken))
            {
                await RefreshBearerToken();
            }

            var json = JsonSerializer.Serialize(new PayUtilityServiceDto { Amount = amount, BankAccountNumber = bankAccountNumber });
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            using var response = await _httpClient.PostAsync($"{UtilityServiceRoute}", payload);

            if (response.StatusCode == HttpStatusCode.Unauthorized && !isRetry)
            {
                await RefreshBearerToken();
                return await PayUtilityServiceAsync(bankAccountNumber, amount, true);
            }

            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();

            var apiResult = await JsonSerializer.DeserializeAsync<ApiResult<float>>(stream, _serializerOptions);

            return apiResult.Result;
        }

        public async Task<bool> CreateUtilityServicePayment(string bankAccountNumber, bool isRetry = false)
        {
            if (string.IsNullOrEmpty(BearerToken))
            {
                await RefreshBearerToken();
            }

            using var response = await _httpClient.PostAsync($"{UtilityServiceRoute}/{bankAccountNumber}", null);

            if (response.StatusCode == HttpStatusCode.Unauthorized && !isRetry)
            {
                await RefreshBearerToken();
                return await CreateUtilityServicePayment(bankAccountNumber, true);
            }

            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();

            var apiResult = await JsonSerializer.DeserializeAsync<ApiResult>(stream, _serializerOptions);

            return apiResult.Success;
        }
    }
}
