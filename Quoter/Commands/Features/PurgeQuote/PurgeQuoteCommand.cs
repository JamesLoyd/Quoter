using Quoter.Commands.Abstractions;
using Quoter.Domain.Models;

namespace Quoter.Commands.Features.PurgeQuote;

public class PurgeQuoteCommand : ICommand<Result<Response>>
{
    public GuildModel Guild { get; set; } = null!;
    public long Id { get; set; }
    public string UserName { get; set; } = "";
}