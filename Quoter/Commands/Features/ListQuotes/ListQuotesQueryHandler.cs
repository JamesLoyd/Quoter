using Quoter.Queries;

namespace Quoter.Commands.Features.ListQuotes;

public class ListQuotesQueryHandler : QueryHandler<ListQuotesQuery, IEnumerable<string>>
{
    protected override Task<IEnumerable<string>> HandleQueryAsync(ListQuotesQuery query, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}