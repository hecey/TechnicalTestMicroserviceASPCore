using AccountService.Clients;
using AccountService.Data;
using AccountService.Repositories;
using Common.Entities;
using Common.Repositories;
using Common.Settings;
using Microsoft.EntityFrameworkCore;

SqlServerSettings sqlServerSettings;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);

sqlServerSettings = builder.Configuration.GetSection(nameof(SqlServerSettings)).Get<SqlServerSettings>();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(sqlServerSettings.DefaultContext ?? throw new InvalidOperationException("Connection string 'DefaultContext' not found.")));
builder.Services.AddHttpClient<RemoteClientService>(client => client.BaseAddress = new Uri("https://localhost:7149/api"));
builder.Services.AddScoped<IAccountRepository<Account>, AccountRepository<Account>>();
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
