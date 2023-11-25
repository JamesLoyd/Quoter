using Quoter.Commands.Abstractions;
using Quoter.Domain.Models;

namespace Quoter.Commands.Features.PurgeQuotes;

public class PurgeQuotesCommand : ICommand<Result<Response>>
{
    public GuildModel Guild { get; set; }
    public string UserName { get; set; }
}