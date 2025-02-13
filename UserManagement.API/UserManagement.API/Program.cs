using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UserManagement.API.Data;
using UserManagement.API.Models;
using UserManagement.API.Services;
using System.Text;
using UserManagement.API.Data;
using UserManagement.API.Models;
using UserManagement.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
           .EnableSensitiveDataLogging() // 显示敏感数据
           .LogTo(Console.WriteLine, LogLevel.Information); // 输出到控制台;
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

// Add Migration
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        // 确保所有挂起的迁移都被应用，并创建数据库（如果它还不存在）
        await context.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

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
