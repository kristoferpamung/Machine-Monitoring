using System.Text;
using MachineMonitoring.Api.Data;
using MachineMonitoring.Api.Hubs;
using MachineMonitoring.Api.Models;
using MachineMonitoring.Api.Repositories;
using MachineMonitoring.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
    // Add JWT authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Add SignalR
builder.Services.AddSignalR();

// Add DbContext
builder.Services.AddDbContext<MachineMonitoringDbContext>(options =>
    options.UseSqlite("Data Source=MachineMonitoring.db"));

    // Configure Microsoft Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Password security settings
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    
    // Lockout protection settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
    
    // User settings
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<MachineMonitoringDbContext>()
.AddDefaultTokenProviders();

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"] ?? 
                throw new InvalidOperationException("JWT SecretKey not configured"))),
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero
    };
});

// DI Repository
builder.Services.AddScoped<IMachineRepository, MachineRepository>();
builder.Services.AddScoped<IOperatorRepository, OperatorRepository>();
builder.Services.AddScoped<IMachineLogRepository, MachineLogRepository>();


// DI Services
builder.Services.AddScoped<IMachineService, MachineService>();
builder.Services.AddScoped<IOperatorService, OperatorService>();
builder.Services.AddScoped<IMachineLogService, MachineLogService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<MachineLogHub>("/hubs/machine-log");

// Initialize database and seeding
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MachineMonitoringDbContext>();
    context.Database.EnsureCreated();
}

app.Run();
