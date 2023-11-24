using Microsoft.EntityFrameworkCore;
using Quoter.Queries;
using Serilog;

namespace Quoter.Commands.Features.ListKeywords;

public class ListKeywordQueryHandler : QueryHandler<ListKeywordQuery, IEnumerable<string>>
{
    private readonly QuoterContext _context;
    private readonly ILogger _logger;

    public ListKeywordQueryHandler(QuoterContext context, ILogger logger)
    {
        _context = context;
        _logger = logger;
    }
    protected override async Task<IEnumerable<string>> HandleQueryAsync(ListKeywordQuery query, CancellationToken cancellationToken = default)
    {
        _logger.Information("Handling {QueryName}", nameof(ListKeywordQuery));
        var keywords = await _context.Quotes.ToListAsync(cancellationToken: cancellationToken);

        var keywords2 = keywords.Where(x => x.GuildId.ToString() == query.Guild.Id.ToString())
            .Select(x => x.KeyWord).ToList();
        
        return keywords2;
    }
}