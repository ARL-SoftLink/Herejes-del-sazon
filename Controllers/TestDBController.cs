using Microsoft.AspNetCore.Mvc;
using herejes_del_sazon.Models;
using Npgsql;

public class TestDbController : Controller
{
    private readonly MyDBContext _dbContext;
    private readonly IConfiguration _config;

    public TestDbController(MyDBContext dbContext, IConfiguration config)
    {
        _dbContext = dbContext;
        _config = config;
    }

    public IActionResult Index()
    {
        // Opción 1: Usar DbContext
        try
        {
            bool canConnect = _dbContext.Database.CanConnect();
            if (canConnect)
                return Content("✅ Conexión exitosa (DbContext)");
        }
        catch (Exception ex)
        {
            return Content($"❌ DbContext error: {ex.Message}");
        }

        // Opción 2: Conexión manual con Npgsql (para ver el error real)
        try
        {
            // Reconstruye la cadena de conexión igual que en Program.cs
            // O la obtienes de la configuración si la guardaste. La reconstruimos:
            var host = "BD";  // o leer de variable de entorno
            var db = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? "HerejesSazonBD";
            var user = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "postgres";
            var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "";
            var connString = $"Host={host};Database={db};Username={user};Password={password}";

            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            return Content("✅ Conexión exitosa (manual)");
        }
        catch (Exception ex)
        {
            return Content($"❌ Error manual: {ex.Message}");
        }
    }
}