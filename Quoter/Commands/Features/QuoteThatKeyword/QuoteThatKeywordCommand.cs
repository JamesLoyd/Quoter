using Quoter.Commands.Abstractions;
using Quoter.Domain.Models;

namespace Quoter.Commands.Features.QuoteThatKeyword;

public class QuoteThatKeywordCommand : IChannelCommand<Result<Response>>
{
    public string? Keyword { get; set; } = "";
    public string? Quote { get; set; } = "";
    public GuildModel Guild { get; set; } = null!;
    public ChannelModel Channel { get; set; } = null!;
}