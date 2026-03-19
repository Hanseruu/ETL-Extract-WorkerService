using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using System.Globalization;
using CsvHelper;
using System.Globalization;
using Microsoft.Data.SqlClient;
using ETLExtractService.Models;

namespace ETLExtractService.Services
{


    public class CsvService
    {
        private readonly ILogger<CsvService> _logger;
        private readonly IConfiguration _config;

        public CsvService(ILogger<CsvService> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public async Task ExtractAsync()
        {
            try
            {
                var connectionString = _config.GetConnectionString("StagingDB");

                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                await InsertCustomers(connection);
                await InsertProducts(connection);
                await InsertOrders(connection);
                await InsertOrderDetails(connection);

                _logger.LogInformation("Todos los CSV fueron cargados a STAGING");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en CSV");
            }
        }

        private async Task InsertCustomers(SqlConnection connection)
        {
            using var reader = new StreamReader("Data/customers.csv");
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<CustomerCsv>();

            foreach (var r in records)
            {
                var query = @"INSERT INTO Stg_Customers 
              (CustomerID, FirstName, LastName, Email, Phone, City, Country)
              VALUES (@CustomerID, @FirstName, @LastName, @Email, @Phone, @City, @Country)";

                using var cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@CustomerID", r.CustomerID);
                cmd.Parameters.AddWithValue("@FirstName", r.FirstName);
                cmd.Parameters.AddWithValue("@LastName", r.LastName);
                cmd.Parameters.AddWithValue("@Email", r.Email);
                cmd.Parameters.AddWithValue("@Phone", r.Phone);
                cmd.Parameters.AddWithValue("@City", r.City);
                cmd.Parameters.AddWithValue("@Country", r.Country);

                await cmd.ExecuteNonQueryAsync();
            }
        }

        private async Task InsertProducts(SqlConnection connection)
        {
            using var reader = new StreamReader("Data/products.csv");
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<ProductCsv>();

            foreach (var r in records)
            {
                var query = @"INSERT INTO Stg_Products 
                        (ProductID, ProductName, Category, Price, Stock)
                        VALUES (@ProductID, @ProductName, @Category, @Price, @Stock)";


                using var cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@ProductID", r.ProductID);
                cmd.Parameters.AddWithValue("@ProductName", r.ProductName);
                cmd.Parameters.AddWithValue("@Category", r.Category);
                cmd.Parameters.AddWithValue("@Price", r.Price);
                cmd.Parameters.AddWithValue("@Stock", r.Stock);

                await cmd.ExecuteNonQueryAsync();
            }
        }

        private async Task InsertOrders(SqlConnection connection)
        {
            using var reader = new StreamReader("Data/orders.csv");
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<OrderCsv>();

            foreach (var r in records)
            {
                var query = @"INSERT INTO Stg_Orders 
                            (OrderID, CustomerID, OrderDate, Status)
                            VALUES (@OrderID, @CustomerID, @OrderDate, @Status)";

                using var cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@OrderID", r.OrderID);
                cmd.Parameters.AddWithValue("@CustomerID", r.CustomerID);
                cmd.Parameters.AddWithValue("@OrderDate", r.OrderDate);
                cmd.Parameters.AddWithValue("@Status", r.Status);

                await cmd.ExecuteNonQueryAsync();
            }
        }

        private async Task InsertOrderDetails(SqlConnection connection)
        {
            using var reader = new StreamReader("Data/order_details.csv");
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<OrderDetailCsv>();

            foreach (var r in records)
            {
                var query = @"INSERT INTO Stg_OrderDetails 
                            (OrderID, ProductID, Quantity, TotalPrice)
                            VALUES (@OrderID, @ProductID, @Quantity, @TotalPrice)";

                using var cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@OrderID", r.OrderID);
                cmd.Parameters.AddWithValue("@ProductID", r.ProductID);
                cmd.Parameters.AddWithValue("@Quantity", r.Quantity);
                cmd.Parameters.AddWithValue("@TotalPrice", r.TotalPrice);

                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
