using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Server.Core.Abstractions;
using Server.Middleware;
using Server.Persistence;
using Server.Persistence.Repositories;
using Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", b =>
    {
        b.AllowAnyMethod()
            .AllowAnyHeader()
            .AllowAnyOrigin();
    });
});

// Add services to the container.

builder.Services.AddSingleton<RsaSecurityKey>(provider =>
{
    var rsa = RSA.Create();
    rsa.ImportRSAPublicKey(Convert.FromBase64String(builder.Configuration["Jwt:PublicKey"]), out _);
    return new RsaSecurityKey(rsa);
});
builder.Services.AddAuthentication(x =>
    {
        x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var rsa = builder.Services.BuildServiceProvider().GetRequiredService<RsaSecurityKey>();
        
        options.IncludeErrorDetails = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = rsa,
            RequireSignedTokens = true,
            RequireExpirationTime = true,
            ValidateLifetime = true,
            ValidateAudience = false,
            ValidateIssuer = false
        };
    });

builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<ISubjectsRepository, SubjectsRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<ISubjectsService, SubjectsService>();

builder.Services.AddScoped<IPasswordManager, PasswordManager>();
builder.Services.AddScoped<IClaimsManager, ClaimsManager>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<SchedulerDbContext>(optionsBuilder => 
    optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("Scheduler")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowAll");

app.Run();
