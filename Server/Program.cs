using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Server.Core.Services;
using Server.Persistence;
using Server.Persistence.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddCors(options =>
{
    options.AddPolicy("CORSPolicy", b =>
    {
        b.AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins("http://localhost:3000", "https://appname.azurestaticapps.net");
    });
});

// Add services to the container.

builder.Services.AddSingleton<RsaSecurityKey>(provider =>
{
    var rsa = RSA.Create();
    rsa.ImportRSAPublicKey(Convert.FromBase64String(builder.Configuration["Jwt:PublicKey"]), out _);
    return new RsaSecurityKey(rsa);
});
builder.Services.AddAuthentication()
    .AddJwtBearer("Asymmetric", options =>
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
builder.Services.AddTransient<ITokenManager, TokenManager>();

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

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors("CORSPolicy");

app.Run();
