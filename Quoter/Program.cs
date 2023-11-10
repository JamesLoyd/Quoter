using Microsoft.EntityFrameworkCore;

namespace Quoter;

public class Program
{
    public static async Task Main()
    {
        var context = new QuoterContext();
        await context.Database.MigrateAsync();
        await new Bot().MainAsync();
    }
}