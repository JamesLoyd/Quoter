using Microsoft.EntityFrameworkCore;

namespace Quoter;

public class Program
{
    public static async Task Main()
    {
        var context = new QuoterContext();
        context.Database.Migrate();
        await new Bot().MainAsync();
    }
}