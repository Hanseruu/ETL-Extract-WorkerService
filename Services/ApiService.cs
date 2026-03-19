using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETLExtractService.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiService> _logger;

        public ApiService(HttpClient httpClient, ILogger<ApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task ExtractAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("comments");

                response.EnsureSuccessStatusCode();

                var data = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("Datos obtenidos desde API");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en API");
            }
        }
    }
}
