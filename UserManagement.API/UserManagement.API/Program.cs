using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UserManagement.Api.Data;
using UserManagement.Api.Models;
using UserManagement.Api.Services;
using System.Text;
using UserManagement.Api.Data;
using UserManagement.Api.Models;
using UserManagement.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<ITokenService, TokenService>();

// For Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHttpClient();

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });

    options.AddPolicy("AllowFrontend", builder =>
    {
        builder.WithOrigins("https://localhost:3000", "http://localhost:3000", "https://localhost:8081") // 
               .AllowCredentials()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });

});

// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    //options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}
)
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,

            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            ClockSkew = TimeSpan.Zero,
            // dotnet user-secrets set "JWT:secret" "your-32-characters-long-super-strong-jwt-secret-key"
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:secret"]))
        };

        //options.Events = new JwtBearerEvents
        //{
        //    OnMessageReceived = context =>
        //    {
        //        context.Request.Cookies.TryGetValue("access_token", out var accessToken);
        //        if (!string.IsNullOrEmpty(accessToken))
        //        {
        //            context.Token = accessToken;
        //        }
        //        return Task.CompletedTask;
        //    }
        //};
    }
);

var app = builder.Build();

// Seed the database
await DbSeeder.SeedData(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Use CORS middleware
app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//// Listen only on HTTP in non-production environments
//if (!app.Environment.IsProduction())
//{
//    app.Urls.Add("https://*:7023");
//}
//else
//{
//    // Configure production-specific bindings if needed
//    app.Urls.Add("https://*:7023");
//}

app.Run();
