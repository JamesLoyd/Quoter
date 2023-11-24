using Quoter.Commands.Abstractions;
using Quoter.Entities;
using Serilog;

namespace Quoter.Commands.Features.QuoteThatKeyword;

public class QuoteThatKeywordCommandHandler : CommandHandler<QuoteThatKeywordCommand, Result<Response>>
{
    private readonly QuoterContext _quoterContext;
    private readonly ILogger _logger;
    public QuoteThatKeywordCommandHandler(QuoterContext context, ILogger logger)
    {
        _quoterContext = context;
        _logger = logger;
    }
    
    protected override async Task<Result<Response>> HandleCommandAsync(QuoteThatKeywordCommand command, CancellationToken cancellationToken = default)
    {
        _logger.Information("Handling {CommandName}", nameof(QuoteThatKeywordCommand));
        if (_quoterContext.Quotes.Any(x => x.KeyWord == command.Keyword))
        {
            return Result.Failure<Response>(new Error("500","Keywords must be unique", true));
        }
        await _quoterContext.Quotes.AddAsync(new Quotes
        {
            KeyWord = command.Keyword!,
            Text = command.Quote!,
            ChannelId = command.Channel.Id.ToString(),
            GuildId = command.Guild.Id.ToString()
        }, cancellationToken);
        await _quoterContext.SaveChangesAsync(cancellationToken);
        _logger.Information("Saved Quoted {Quote} with keyword {Keyword}", command.Quote, command.Keyword);
        return Result.Success(new Response
        {
            Message = $"Quoted {command.Quote}, look it up via keyword: {command.Keyword}"
        });
    }
}