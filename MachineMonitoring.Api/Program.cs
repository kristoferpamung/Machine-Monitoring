using MachineMonitoring.Api.Data;
using MachineMonitoring.Api.Repositories;
using MachineMonitoring.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
