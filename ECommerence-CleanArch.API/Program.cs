using ECommerence_CleanArch.Infrastructure;
using ECommerence_CleanArch.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Infrastructure Layer (DbContext, Repositories, UnitOfWork)
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddApplicationServices();

builder.Services.AddControllers();

// OpenAPI konfigürasyonu
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
