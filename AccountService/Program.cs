using AccountService.Clients;
using AccountService.Data;
using AccountService.Repositories;
using Common.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});
// Add services to the container.
builder.Services.AddDbContext<DataContext>(o => o.UseInMemoryDatabase("AccountDB"));
builder.Services.AddHttpClient<RemoteClientService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7149/api");
});

builder.Services.AddScoped<AccountRepository<Account>, AccountRepository<Account>>();
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
