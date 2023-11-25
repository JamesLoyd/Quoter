using Quoter.Commands.Abstractions;
using Quoter.Domain.Models;

namespace Quoter.Commands.Features.QuoteThat;

public class QuoteThatCommand : IChannelUserCommand<Result<Response>>
{
    public GuildModel Guild { get; set; }
    public ChannelModel Channel { get; set; }
    public UserModel User { get; set; }
    public string Quote { get; set; }
}