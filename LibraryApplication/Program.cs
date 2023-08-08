using LibraryApplication.Data;
using LibraryApplication.Helper;
using LibraryApplication.Interfaces;
using LibraryApplication.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var cloudinarySettings = builder.Configuration.GetSection("Cloudinary");
builder.Services.Configure<CloudinarySettings>(cloudinarySettings);

builder.Services.AddTransient<PhotoService>();
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<BorrowService>();
builder.Services.AddScoped<IBookRepository, BookService>();
builder.Services.AddScoped<IUserRepository, UserService>();
builder.Services.AddScoped<IBorrowRepository, BorrowService>();

var app = builder.Build();

// Hata Yönetimi Middleware'i
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Book}/{action=Index}/{id?}");

app.Run();
