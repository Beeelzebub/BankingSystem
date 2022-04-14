using BankingSystem.Common.DTO;
using Microsoft.Extensions.Configuration;
using IdentityModel.Client;
using System.Text.Json;
using System.Net;

namespace BankingSystem.Common.HttpClients
{
    public abstract class HttpClientBase
    {
        protected readonly HttpClient _httpClient;
        protected readonly JsonSerializerOptions _serializerOptions;
        private readonly HttpClient _authorizationHttpClient;
        private readonly IConfiguration _configuration;
        protected string BearerToken;

        public HttpClientBase(HttpClient httpClient, IConfiguration configuration, string url)
        {
            _httpClient = httpClient;
            _authorizationHttpClient = new HttpClient();
            _configuration = configuration;
            _serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            ConfigureClient(httpClient, url);
        }

        protected async Task<T> GetAsync<T>(string urn)
        {
            return await GetAsyncWithRetry<T>(urn);
        }

        private async Task<T> GetAsyncWithRetry<T>(string urn, bool isRetry = false)
        {
            if (string.IsNullOrEmpty(BearerToken))
            {
                await RefreshBearerToken();
            }

            using var response = await _httpClient.GetAsync(urn);

            if (response.StatusCode == HttpStatusCode.Unauthorized && !isRetry)
            {
                await RefreshBearerToken();
                return await GetAsyncWithRetry<T>(urn, true);
            }

            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();

            var apiResult = await JsonSerializer.DeserializeAsync<ApiResult<T>>(stream, _serializerOptions);

            return apiResult.Result;
        }

        private void ConfigureClient(HttpClient httpClient, string url)
        {
            _httpClient.BaseAddress = new Uri(url);
            _httpClient.Timeout = new TimeSpan(0, 0, 30);
        }

        protected async Task RefreshBearerToken()
        {
            var tokenResponse = await _authorizationHttpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = _configuration["TokenEndpoint"],
                ClientId = _configuration["ClientId"],
                ClientSecret = _configuration["ClientSecret"],
                Scope = _configuration["Scope"]
            });

            if (tokenResponse.IsError)
            {
                throw new HttpRequestException("Token refresh request failed");
            }

            BearerToken = tokenResponse.AccessToken;

            _httpClient.SetBearerToken(tokenResponse.AccessToken);
        }
    }
}
