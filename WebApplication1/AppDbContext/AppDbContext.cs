using Microsoft.EntityFrameworkCore;
using Domain;

public class AppDbContext : DbContext
{
    public DbSet<Author> Users { get; set; } // Таблица Users

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}