using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using restapi_crud_practice.Data;
using restapi_crud_practice.Entities;
using restapi_crud_practice.Helpers;
using restapi_crud_practice.Repositories;
using restapi_crud_practice.Repositories.RBook;
using restapi_crud_practice.Repositories.RBorrow;
using restapi_crud_practice.Repositories.RClient;
using restapi_crud_practice.Services.SAuth;
using restapi_crud_practice.Services.SBook;
using restapi_crud_practice.Services.SBorrow;
using restapi_crud_practice.Services.SClient;
using restapi_crud_practice.Services.SJwt;
using restapi_crud_practice.Services.SToken;
using restapi_crud_practice.Services.SUserContext;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var connString = builder.Configuration.GetConnectionString("connKey");
builder.Services.AddDbContext<BookBorrowingContext>(options => options.UseNpgsql(connString));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["AppSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["AppSettings:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!)),
            ValidateIssuerSigningKey = true
        };
    });
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBorrowService, BorrowService>();
builder.Services.AddScoped<IBorrowRepository, BorrowRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddScoped<BorrowHelper>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddScoped<IJwtSettingsService, JwtSettingsService>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });

    var xml = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xml);
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath);

    var jwtScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT access token}"
    };

    c.AddSecurityDefinition("Bearer", jwtScheme);
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
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
        c.RoutePrefix = "swagger"; // browse at /swagger
    });
}

app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var authService = services.GetRequiredService<IAuthService>();
        await DataSeeder.SeedAdminUserAsync(authService, builder.Configuration);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Seeding error: {ex.Message}");
        Console.WriteLine($"Error details: {ex}");
    }
}

await app.RunAsync();