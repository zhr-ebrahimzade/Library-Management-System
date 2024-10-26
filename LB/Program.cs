using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
//Add Services into Ioc container
builder.Services.AddScoped<IBooksService, BooksService>();
builder.Services.AddScoped<IBorrowersService, BorrowersService>();
builder.Services.AddDbContext<LibraryDbContext>(options =>
{ options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); });

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();