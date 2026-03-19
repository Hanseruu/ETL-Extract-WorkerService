using ETLExtractService.Services;
namespace ETLExtractService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _services;

        public Worker(ILogger<Worker> logger, IServiceProvider services)
        {
            _logger = logger;
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Iniciando proceso de extracción de datos...");
            
            using var scope = _services.CreateScope();

            var csvService = scope.ServiceProvider.GetRequiredService<CsvService>();
            var apiService = scope.ServiceProvider.GetRequiredService<ApiService>();
            var dbService =  scope.ServiceProvider.GetRequiredService<DatabaseService>();
           
            await csvService.ExtractAsync();
            await apiService.ExtractAsync();
           // await dbService.ExtractAsync();

            _logger.LogInformation("Extracción completada.");
        }
    }
}
