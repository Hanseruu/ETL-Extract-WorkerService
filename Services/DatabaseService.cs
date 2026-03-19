using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETLExtractService.Services
{
    public class DatabaseService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<DatabaseService> _logger;

        public DatabaseService(IConfiguration config, ILogger<DatabaseService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task ExtractAsync()
        {
            try
            {
                var connectionString = _config.GetConnectionString("StagingDB");

                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                var query = "SELECT * FROM Reviews"; // ejemplo

                using var command = new SqlCommand(query, connection);
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    // Leer datos
                }

                _logger.LogInformation("Datos extraídos desde BD");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en BD");
            }
        }
    }
}
