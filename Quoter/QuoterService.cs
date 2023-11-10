using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Quoter;

public class QuoterService : IHostedService
{
    private readonly IBot _bot;
    private readonly QuoterContext _quoterContext;
    public QuoterService(IBot bot, QuoterContext quoterContext)
    {
        _bot = bot;
        _quoterContext  = quoterContext;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Starting now and migrating");
        await _quoterContext.Database.MigrateAsync(cancellationToken: cancellationToken);
        Console.WriteLine("Bot Starting now");
        await _bot.MainAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Stopping now");
        return Task.CompletedTask;
    }
}