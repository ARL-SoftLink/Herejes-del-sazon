using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using herejes_del_sazon.Models;
using Npgsql;
using herejes_del_sazon.Services;

// leer variables de entorno y establecer valores proporcionados a la cadena de conexion
var envPath = Path.Combine(AppContext.BaseDirectory, ".env");
var host = Environment.GetEnvironmentVariable("DATABASE_HOST") ?? "host.docker.internal";
var port = Environment.GetEnvironmentVariable("DATABASE_PORT") ?? "5432";
var db = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? "HerejesSazonBD";
var user = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "postgres";
var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "";


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// leer variables de entorno
builder.Services.AddScoped<MenuService>();

// cadena de conexion
var connectionString = $"Host={host};Port={port};Database={db};Username={user};Password={password}";

// Configurar PostgreSQL con DbContext vacío
builder.Services.AddDbContext<MyDBContext>(options =>
    options.UseNpgsql(connectionString)
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
