using Quoter.Commands.Abstractions;
using Quoter.Domain.Models;

namespace Quoter.Commands.Features.DeleteQuoteKeyword;

public class DeleteQuoteKeywordCommand : IGuildCommand<Result<Response>>
{
    public string Keyword { get; set; } = "";
    public GuildModel Guild { get; set; } = null!;
}