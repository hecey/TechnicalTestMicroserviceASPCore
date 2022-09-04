using Microsoft.EntityFrameworkCore;
using TTM.ClientService.Data;
using TTM.ClientService.Repositories;
using TTM.ClientService.UnitOfWork;
using TTM.Common.Entities;
using TTM.Common.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});
// Add services to the container.
builder.Services.AddDbContext<DataContext>(o => o.UseInMemoryDatabase("ClientsDB"));

builder.Services.AddScoped<ClientRepository<Client>, ClientRepository<Client>>();
builder.Services.AddScoped<IUnitOfWork<ClientRepository<Client>>, UnitOfWork<ClientRepository<Client>>>();
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
