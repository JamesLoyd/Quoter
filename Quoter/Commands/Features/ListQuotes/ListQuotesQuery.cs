using Quoter.Domain.Models;
using Quoter.Queries;

namespace Quoter.Commands.Features.ListQuotes;

public class ListQuotesQuery : IQuery<IEnumerable<string>>
{
    public GuildModel Guild { get; set; }
    public string UserName { get; set; } = "";
}