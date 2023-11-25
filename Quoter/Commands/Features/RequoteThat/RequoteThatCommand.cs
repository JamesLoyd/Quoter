using Quoter.Commands.Abstractions;
using Quoter.Domain.Models;

namespace Quoter.Commands.Features.RequoteThat;

public class RequoteThatCommand : IUserCommand<Result<Response>>
{
    public GuildModel Guild { get; set; }
    public UserModel User { get; set; }
}