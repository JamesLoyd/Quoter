using System.ComponentModel.DataAnnotations;
using Discord.WebSocket;
using MediatR;
using Quoter.Commands;
using Quoter.Commands.Abstractions;
using Quoter.Commands.Features.ListKeywords;
using Quoter.Commands.Features.QuoteThatKeyword;
using Quoter.Domain.Models;
using Serilog;
using ValidationException = Quoter.Validation.ValidationException;

namespace Quoter;

public interface ICommandDispatcher
{
    Task<Result<Response>> DispatchCommand(SocketSlashCommand command);
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

    public async Task<Result<Response>> DispatchCommand(SocketSlashCommand command)
    {
        var commandFound = _commandRegistrations.SingleOrDefault(x => x.CommandName == command.Data.Name);
        if (commandFound == null)
        {
            return new Response
            {
                Message = "Command not found",
                Ephemeral = true
            };
        }

        _logger.Information(" Found {CommandName}", commandFound.CommandType.Name);
        if (commandFound.CommandType == typeof(QuoteThatKeywordCommand))
        {
            return await HandleQuoteThatKeyword(command, commandFound);
        }

        if (commandFound.CommandType == typeof(ListKeywordQuery))
        {
            var response = await _mediator.Send(new ListKeywordQuery
            {
                Guild = new GuildModel
                {
                    Id = command.GuildId.GetValueOrDefault()
                }
            });
            return Result.Success(new Response
            {
                Message = string.Join(',', response),
                Ephemeral = true
            });
        }

        throw new Exception();
    }

    private async Task<Result<Response>> HandleQuoteThatKeyword(SocketSlashCommand command,
        ICommandRegistration commandFound)
    {
        _logger.Information("Dispatching {CommandName}", commandFound.CommandName);
        var id = command.Data.Options.ElementAt(0).Value! as string;
        var keyword = command.Data.Options.ElementAt(1).Value! as string;

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
            return response;
        }
        catch (ValidationException e)
        {
            _logger.Error(e, "Error: {Error}", string.Join(",", e.Errors));
            return Result.Failure<Response>(new Error("500", string.Join(",", e.Errors.Select(x => x.ErrorMessage)), true));
        }
        catch (Exception e)
        {
            _logger.Error(e, "Error: {Error}", e.Message);
            return Result.Failure<Response>(new Error("500", e.Message, true));
        }
    }
}