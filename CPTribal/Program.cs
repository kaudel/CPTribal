using AspNetCoreRateLimit;
using CPTribal.BussinesRules;
using CPTribal.Data;
using CPTribal.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();

builder.Services.AddScoped<CreditParameter>();
//builder.Services.AddScoped<IDbAccess, DbAccess>();
builder.Services.AddScoped<BaseCreditLine, CreditSME>();
builder.Services.AddScoped<BaseCreditLine, CreditStartup>();
builder.Services.AddScoped<ICreditValidation, CreditValidation>();
builder.Services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase("CreditEvaluation"));
builder.Services.AddScoped<IDbAccess, DbAccess>();

builder.Services.AddOptions();
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(configuration.GetSection("IprateLimitPolice"));
builder.Services.AddInMemoryRateLimiting();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseIpRateLimiting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();



app.Run();
