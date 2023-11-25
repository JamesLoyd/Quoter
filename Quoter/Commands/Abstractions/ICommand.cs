using Discord.WebSocket;
using MediatR;
using Quoter.Domain.Interfaces;

namespace Quoter.Commands.Abstractions;

public interface ICommand : ICommand<Unit>
{
}

public interface ICommand<out T> : IRequest<T>
    where T : notnull
{
}

public interface IGuildCommand<out T> : ICommand<T>, IHasGuild
    where T : notnull
{
}

public interface IUserCommand<out T> : ICommand<T>, IGuildCommand<T>
    where T : notnull
{
    
}



public interface IChannelCommand<out T> : IGuildCommand<T>, IHasChannel
    where T : notnull
{
}