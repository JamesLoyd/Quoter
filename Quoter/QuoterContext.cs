using Microsoft.EntityFrameworkCore;
using Quoter.Entities;

namespace Quoter;

public class QuoterContext : DbContext
{
    public DbSet<QuoteRecord> QuoteRecords { get; set; } = null!;
    public DbSet<Permission> Permissions { get; set; } = null!;
    private string DbPath { get; }

    public QuoterContext()
    {
        const Environment.SpecialFolder folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder)+"/quoter/";
        if (!System.IO.Directory.Exists(path))
        {
            System.IO.Directory.CreateDirectory(path);
        }
        DbPath = System.IO.Path.Join(path, "Quoter.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite($"Data Source={DbPath}");
}