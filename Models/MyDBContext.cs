// Data/PlaceholderDbContext.cs
using Microsoft.EntityFrameworkCore;

namespace herejes_del_sazon.Models
{
    public class MyDBContext : DbContext
    {
        public MyDBContext(DbContextOptions<MyDBContext> options) 
            : base(options) { }

        // Intencionalmente sin DbSets
    }
}