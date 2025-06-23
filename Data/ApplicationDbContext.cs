using areayvolumen.Models;
using Microsoft.EntityFrameworkCore;

namespace areayvolumen.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CalculationHistory> CalculationHistories { get; set; }
    }
} 