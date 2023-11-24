using Discord.WebSocket;
using MediatR;
using Quoter.Commands;
using Quoter.Commands.Abstractions;
using Quoter.Commands.Features.QuoteThatKeyword;
using Quoter.Domain.Models;
using Serilog;

namespace Quoter;

public interface ICommandDispatcher
{
    Task<Response> DispatchCommand(SocketSlashCommand command);
}

public class CommandDispatcher : ICommandDispatcher
{
    private readonly IMediator _mediator;
    private readonly IEnumerable<ICommandRegistration> _commandRegistrations;
    private readonly ILogger _logger;

    public CommandDispatcher(IMediator mediator, IEnumerable<ICommandRegistration> commandRegistrations, ILogger logger)
    {
        _mediator = mediator;
        _commandRegistrations = commandRegistrations;
        _logger = logger;
    }

    public async Task<Response> DispatchCommand(SocketSlashCommand command)
    {
        var commandFound = _commandRegistrations.SingleOrDefault(x => x.Name == command.Data.Name);
        if (commandFound == null)
        {
            return new Response
            {
                Message = "Command not found",
                Ephemeral = true
            };
        }

        _logger.Information(" Found {CommandName}", commandFound.Name);
        if (commandFound.CommandType == typeof(QuoteThatKeywordCommand))
        {
            _logger.Information("Dispatching {CommandName}", commandFound.Name);
            var id = command.Data.Options.ElementAt(0).Value! as string;
            var keyword = command.Data.Options.ElementAt(1).Value! as string;
            if (id.Contains("<@"))
            {
                return new Response
                {
                    Message = "Mentions are not allowed",
                    Ephemeral = true
                };
            }

            _logger.Information(":id: {Id} :keyword: {Keyword}", id, keyword);
            try
            {
                var response = await _mediator.Send(new QuoteThatKeywordCommand
                {
                    Channel = new ChannelModel
                    {
                        Id = command.ChannelId.GetValueOrDefault(),
                    },
                    Guild = new GuildModel
                    {
                        Id = command.GuildId.GetValueOrDefault()
                    },
                    Keyword = keyword,
                    Quote = id
                });

                _logger.Information("Response: {Response}", response.Value.Message);
                return response.Value;
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error dispatching {CommandName}", commandFound.Name);
                return new Response
                {
                    Message = "Something went wrong",
                    Ephemeral = true
                };
            }
        }

        throw new Exception();
    }
}