using Quoter.Commands.Abstractions;

namespace Quoter.Commands.Features.AddPermission;

public class AddPermissionCommandHandler : CommandHandler<AddPermissionCommand, Result<Response>>
{
    protected override Task<Result<Response>> HandleCommandAsync(AddPermissionCommand command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}