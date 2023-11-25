using Discord.WebSocket;
using MediatR;
using Quoter.Domain.Interfaces;

namespace Quoter.Commands.Abstractions;

public interface ICommand : ICommand<Unit>
{
}

public interface ICommand<out T> : IRequest<T>, IHasGuild
    where T : notnull
{
}

public interface IUserCommand<out T> : ICommand<T>, IHasUser
    where T : notnull
{
}

public interface IChannelCommand<out T> : ICommand<T>, IHasChannel
    where T : notnull
{
}