using Microsoft.EntityFrameworkCore;
using restapi_crud_practice.Data;
using restapi_crud_practice.Endpoints;
using restapi_crud_practice.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BookBorrowingContext>(options => options.UseNpgsql("Host=localhost;Port=5432;Database=GameStore;Username=postgres;Password=admin"));
builder.Services.AddScoped<IBorrowService, TestService>();
var app = builder.Build();
app.MapClientEndpoints();
app.MapBookEndpoints();
app.MapBorrowEndpoints();

app.Run();
