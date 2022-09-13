using Hecey.TTM.Common.Entities;
using Hecey.TTM.Common.Repositories;
using Hecey.TTM.Common.Settings;
using Microsoft.EntityFrameworkCore;
using TransactionService.Clients;
using TransactionService.Data;
using TransactionService.Repositories;

var isDevelopment=true;
SqlServerSettings sqlServerSettings;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);

sqlServerSettings = builder.Configuration.GetSection(nameof(SqlServerSettings)).Get<SqlServerSettings>();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(sqlServerSettings.DefaultContext ?? throw new InvalidOperationException("Connection string 'DefaultContext' not found.")));
builder.Services.AddHttpClient<RemoteAccountService>(client => client.BaseAddress = new Uri($"https://{sqlServerSettings.Host}:7002/api"))
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler() {
                    // Return `true` to allow certificates that are untrusted/invalid
                    ServerCertificateCustomValidationCallback = (isDevelopment)?HttpClientHandler.DangerousAcceptAnyServerCertificateValidator:null
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
