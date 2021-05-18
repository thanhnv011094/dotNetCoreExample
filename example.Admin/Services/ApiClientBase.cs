using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace example.Admin.Services
{
    public class ApiClientBase
    {
        public ApiClientBase(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        protected IHttpClientFactory _httpClientFactory { get; }
        protected IHttpContextAccessor _httpContextAccessor { get; }
        protected IConfiguration _configuration { get; }

        public async Task<TResponse> GetAsync<TResponse>(string url)
        {
            var client = CreateHttpClient();
            var response = await client.GetAsync(url);
            var body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResponse>(body);
        }

        public async Task<TResponse> PostAsync<TResponse>(string url, object data)
        {
            var client = CreateHttpClient();
            var response = await client.PostAsync(url, GetJsonData(data));
            var body = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            return JsonSerializer.Deserialize<TResponse>(body, options);
        }

        private HttpClient CreateHttpClient()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["ApiBaseUrl"]);
            if(_httpContextAccessor.HttpContext.Session != null)
            {
                var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }

            return client;
        }

        private StringContent GetJsonData(object data)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            var jsonData = JsonSerializer.Serialize(data, options);
            return new StringContent(jsonData, Encoding.UTF8, "application/json");
        }
    }
}
