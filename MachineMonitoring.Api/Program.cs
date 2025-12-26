using MachineMonitoring.Api.Data;
using MachineMonitoring.Api.Repositories;
using MachineMonitoring.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Machine Monitoring API",
        Version = "v1",
        Description = "API for real-time machine production monitoring",
        Contact = new OpenApiContact
        {
            Name = "Kristofer"
        }
    });
} );

builder.Services.AddDbContext<MachineMonitoringDbContext>(options =>
    options.UseSqlite("Data Source=MachineMonitoring.db"));

// DI Repository
builder.Services.AddScoped<IMachineRepository, MachineRepository>();
builder.Services.AddScoped<IOperatorRepository, OperatorRepository>();
builder.Services.AddScoped<IMachineLogRepository, MachineLogRepository>();

// DI Services
builder.Services.AddScoped<IMachineService, MachineService>();
builder.Services.AddScoped<IOperatorService, OperatorService>();
builder.Services.AddScoped<IMachineLogService, MachineLogService>();


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
