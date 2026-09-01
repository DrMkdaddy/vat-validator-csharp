using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace RapidApi.VatValidator
{
    /// <summary>
    /// Configuration options for the VatValidator client.
    /// </summary>
    public class RapidApiConfig
    {
        /// <summary>
        /// RapidAPI Key ('x-rapidapi-key').
        /// Obtain your key at: https://rapidapi.com/noor-mkdad-apis-noor-mkdad-apis-default/api/eu-uk-vat-validator-rate-engine
        /// </summary>
        public string? ApiKey { get; set; }

        /// <summary>
        /// Base URL override. Defaults to https://eu-uk-vat-validator-rate-engine.p.rapidapi.com.
        /// </summary>
        public string BaseUrl { get; set; } = "https://eu-uk-vat-validator-rate-engine.p.rapidapi.com";

        /// <summary>
        /// RapidAPI Host header. Defaults to eu-uk-vat-validator-rate-engine.p.rapidapi.com.
        /// </summary>
        public string RapidApiHost { get; set; } = "eu-uk-vat-validator-rate-engine.p.rapidapi.com";
    }

    /// <summary>
    /// Response envelope from RapidAPI Hub.
    /// </summary>
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? Error { get; set; }
        public string? Code { get; set; }
        public string? SubscribeUrl { get; set; }
        public string? UpgradeUrl { get; set; }
    }

    /// <summary>
    /// High-performance client for EU & UK VAT Validator & Rate Engine.
    /// </summary>
    public class VatValidatorClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly RapidApiConfig _config;
        private readonly bool _disposeClient;

        public VatValidatorClient(RapidApiConfig? config = null, HttpClient? httpClient = null)
        {
            _config = config ?? new RapidApiConfig();
            if (string.IsNullOrEmpty(_config.ApiKey))
            {
                _config.ApiKey = Environment.GetEnvironmentVariable("RAPIDAPI_KEY");
            }

            if (httpClient != null)
            {
                _httpClient = httpClient;
                _disposeClient = false;
            }
            else
            {
                _httpClient = new HttpClient();
                _disposeClient = true;
            }
        }

        public async Task<string> RequestRawAsync(string endpoint, HttpMethod method, string? jsonBody = null, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(_config.ApiKey))
            {
                throw new InvalidOperationException("RapidAPI API Key is required. Obtain a key at: https://rapidapi.com/noor-mkdad-apis-noor-mkdad-apis-default/api/eu-uk-vat-validator-rate-engine");
            }

            var cleanBase = _config.BaseUrl.TrimEnd('/');
            var cleanPath = endpoint.TrimStart('/');
            var requestUrl = $"{cleanBase}/{cleanPath}";

            using var request = new HttpRequestMessage(method, requestUrl);
            request.Headers.Add("x-rapidapi-key", _config.ApiKey);
            request.Headers.Add("x-rapidapi-host", _config.RapidApiHost);

            if (!string.IsNullOrEmpty(jsonBody))
            {
                request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            }

            using var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        public async Task<ApiResponse<JsonElement>> ValidateAsync(object payload, CancellationToken cancellationToken = default)
        {
            var json = JsonSerializer.Serialize(payload);
            var res = await RequestRawAsync("/api/v1/validate", HttpMethod.Post, json, cancellationToken).ConfigureAwait(false);
            return JsonSerializer.Deserialize<ApiResponse<JsonElement>>(res, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                   ?? new ApiResponse<JsonElement> { Success = false, Error = "Failed to deserialize response" };
        }

        public async Task<ApiResponse<JsonElement>> GetHealthAsync(CancellationToken cancellationToken = default)
        {
            var res = await RequestRawAsync("/health", HttpMethod.Get, null, cancellationToken).ConfigureAwait(false);
            return JsonSerializer.Deserialize<ApiResponse<JsonElement>>(res, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                   ?? new ApiResponse<JsonElement> { Success = false, Error = "Failed to deserialize response" };
        }

        public void Dispose()
        {
            if (_disposeClient)
            {
                _httpClient.Dispose();
            }
        }
    }
}
