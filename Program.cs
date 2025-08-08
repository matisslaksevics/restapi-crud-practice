using Microsoft.EntityFrameworkCore;
using restapi_crud_practice.Data;
using restapi_crud_practice.Endpoints;
using restapi_crud_practice.Services.SBook;
using restapi_crud_practice.Services.SBorrow;
using restapi_crud_practice.Services.SClient;

var builder = WebApplication.CreateBuilder(args);
var connString = builder.Configuration.GetConnectionString("connKey");
builder.Services.AddDbContext<BookBorrowingContext>(options => options.UseNpgsql(connString));
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBorrowService, BorrowService>();
var app = builder.Build();
app.MapClientEndpoints();
app.MapBookEndpoints();
app.MapBorrowEndpoints();

app.Run();
