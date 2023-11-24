using Quoter.Domain.Interfaces;
using Quoter.Domain.Models;
using Quoter.Queries;

namespace Quoter.Commands.Features.ListKeywords;

public class ListKeywordQuery : IQuery<IEnumerable<string>>, IHasGuild
{
    public GuildModel Guild { get; set; } = null!;
}