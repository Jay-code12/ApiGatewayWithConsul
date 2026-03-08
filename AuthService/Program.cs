using AuthService;
using AuthService.Modal;
using AuthService.Modal.data;
using AuthService.Modal.settings;
using AuthService.Repositories;
using AuthService.Repositories.Interfaces;
using Consul;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<Auth, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddHttpClient();
builder.Services.AddScoped<ConsulServiceDiscovery>();
builder.Services.AddScoped<UserServiceClient>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.Configure<ConsulSetting>(builder.Configuration.GetSection("ConsulSettings"));
builder.Services.AddSingleton<ConsulRegistration>();
builder.Services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(cfg =>
{
    cfg.Address = new Uri("http://localhost:8500");
}));
builder.Services.AddHealthChecks();

var jwtSetting = builder.Configuration.GetSection("JwtSettings").Get<JwtSetting>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer("Bearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSetting.Issuer,
        ValidAudience = jwtSetting.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSetting.Secret))
    };
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
        });
});

var app = builder.Build();

var registrationWithConsul = app.Services.GetRequiredService<ConsulRegistration>();
registrationWithConsul.RegistrationService(app);

app.UseCors();
app.UseHttpsRedirection();

// Expose the health check endpoint at a specific URL, e.g., "/health" or "/healthz"
app.MapHealthChecks("/health");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
