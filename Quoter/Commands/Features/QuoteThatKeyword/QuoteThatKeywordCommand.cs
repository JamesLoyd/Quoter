using Quoter.Commands.Abstractions;
using Quoter.Domain.Models;

namespace Quoter.Commands.Features.QuoteThatKeyword;

public class QuoteThatKeywordCommand : IGuildCommand<Result<Response>>
{
    public string Keyword { get; set; } = "";
    public string Quote { get; set; } = "";
    public UserModel User { get; set; } = null!;
    public GuildModel Guild { get; set; } = null!;
}