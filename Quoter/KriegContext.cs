using Microsoft.EntityFrameworkCore;
using Quoter;

public class KriegContext : DbContext
{
    public DbSet<QuoteRecord> QuoteRecords { get; set; }
    public string DbPath { get; }

    public KriegContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder)+"/krieg/";
        if (!System.IO.Directory.Exists(path))
        {
            System.IO.Directory.CreateDirectory(path);
        }
        DbPath = System.IO.Path.Join(path, "Krieg.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite($"Data Source={DbPath}");
}