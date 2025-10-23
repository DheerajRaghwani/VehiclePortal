using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using VehiclePortal.Interface;
using VehiclePortal.Service;
using VehiclePortal.Models;


var builder = WebApplication.CreateBuilder(args);

// ✅ JWT Secret
var key = builder.Configuration["Jwt:Key"] ?? "MySuperSecretKeyForJWT123456789012"; // ✅ 32+ chars


// ✅ DbContext
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<VehicleContext>(o =>
    o.UseMySql(conn, ServerVersion.Create(8, 0, 0, Pomelo.EntityFrameworkCore.MySql.Infrastructure.ServerType.MySql))
);

// ✅ Services
builder.Services.AddScoped<IUserLoginService, UserLoginService>();
builder.Services.AddScoped<IVehicleregistration, VehicleregistrationService>();
builder.Services.AddScoped<IBlock, BlockService>();
builder.Services.AddScoped<ISourceService, SourceService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IDistrictDashboardService, DistrictDashboardService>();
builder.Services.AddScoped<ICheckpostDashboard,CheckpostDashboardService>();
builder.Services.AddScoped<ICheckpostService, CheckpostService>();
builder.Services.AddScoped<ICheckpostname, CheckpostnameService>();
builder.Services.AddScoped<IDistrict,DistrictService>();
// ✅ JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// ✅ Swagger with JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "VehiclePortal API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer {token}'"
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
            new string[]{}
        }
    });
});

// ✅ CORS
builder.Services.AddCors(o => o.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "VehiclePortal API v1");
        c.RoutePrefix = string.Empty; // Makes Swagger UI available at https://localhost:7005/
    });
}


app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseStaticFiles();       // If you want to serve index.html or other static files
app.UseAuthentication();    // Must come before UseAuthorization
app.UseAuthorization();

app.MapControllers();
app.Run();
