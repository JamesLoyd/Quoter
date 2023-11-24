using Discord.WebSocket;
using MediatR;
using Quoter.Domain.Interfaces;

namespace Quoter.Commands.Abstractions;

public interface ICommand : ICommand<Unit>
{
}

public interface ICommand<out T> : IRequest<T>, IHasUser
    where T : notnull
{
}

public interface IGuildCommand<out T> : ICommand<T>, IHasGuild
    where T : notnull
{
}

public interface IChannelCommand<out T> : IGuildCommand<T>, IHasChannel
    where T : notnull
{
}