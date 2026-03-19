using ETLExtractService;
using ETLExtractService.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddScoped<CsvService>();
builder.Services.AddScoped<ApiService>();
builder.Services.AddScoped<DatabaseService>();

builder.Services.AddHttpClient<ApiService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]);
});
var host = builder.Build();
host.Run();
