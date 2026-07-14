using IdentityHub.Data; 
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

using System.Text.Json.Serialization;


using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "IdentityHub",
            ValidAudience = "IdentityHub.Applications",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YOUR_VERY_SECRET_KEY_1234567890_MUST_BE_LONG"))
        };
    });


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<IdentityDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers().AddJsonOptions(x =>
   x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Identity Hub API",
        Version = "2026.v1",
        Description = "Central Application Registry & User Management"
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularTemplate",
        policy => policy.WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity Hub v1");
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAngularTemplate");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
