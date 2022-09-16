using Hecey.TTM.Common.Entities;
using Hecey.TTM.Common.Settings;
using Microsoft.EntityFrameworkCore;
using TransactionService.Clients;
using TransactionService.Data;
using TransactionService.Repositories;
using Polly;
using Polly.Timeout;

var jitter = new Random();
var isDevelopment = Environment.GetEnvironmentVariable("isDevelopment");
SqlServerSettings sqlServerSettings = new()
{
    Host = Environment.GetEnvironmentVariable("Host"),
    Port = Environment.GetEnvironmentVariable("Port"),
    Database = Environment.GetEnvironmentVariable("Database"),
    UserId = Environment.GetEnvironmentVariable("UserId"),
    Password = Environment.GetEnvironmentVariable("Password")
};
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);

sqlServerSettings = builder.Configuration.GetSection(nameof(SqlServerSettings)).Get<SqlServerSettings>();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(sqlServerSettings.DefaultContext ?? throw new InvalidOperationException("Connection string 'DefaultContext' not found.")));
builder.Services.AddHttpClient<RemoteAccountService>(client => client.BaseAddress = new Uri($"https://{sqlServerSettings.Host}:{Environment.GetEnvironmentVariable("RemoteAccountServiceHTTPSPort")}/api"))
                .AddTransientHttpErrorPolicy(builder => builder.Or<TimeoutRejectedException>().WaitAndRetryAsync(
                    5,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    + TimeSpan.FromMilliseconds(jitter.Next(0,1000))
                ))
                .AddTransientHttpErrorPolicy(builder => builder.Or<TimeoutRejectedException>().CircuitBreakerAsync(
                    3,
                    TimeSpan.FromSeconds(15)
                ))
                .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(1))
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
                {
                    // Return `true` to allow certificates that are untrusted/invalid
                    ServerCertificateCustomValidationCallback = (isDevelopment == "true") ? HttpClientHandler.DangerousAcceptAnyServerCertificateValidator : null
                });

builder.Services.AddScoped<ITransactionRepository<Transaction>, TransactionRepository<Transaction>>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
