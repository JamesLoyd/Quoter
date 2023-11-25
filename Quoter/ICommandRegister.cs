using Discord;
using Discord.WebSocket;
using Quoter.Commands.Abstractions;
using Serilog;

namespace Quoter;

public interface ICommandRegister
{
    Task RegisterCommandsAsync(DiscordSocketClient client);
}

public class CommandRegister : ICommandRegister
{
    private readonly IEnumerable<ICommandRegistration> _commandRegistrations;
    private readonly ILogger _logger;

    public CommandRegister(IEnumerable<ICommandRegistration> commandRegistrations, ILogger logger)
    {
        _commandRegistrations = commandRegistrations;
        _logger = logger;
    }

    public async Task RegisterCommandsAsync(DiscordSocketClient client)
    {
        List<ApplicationCommandProperties> applicationCommandProperties = new();
        _logger.Information("I have {Count} to register", _commandRegistrations.Count());
        foreach (var commandRegistration in _commandRegistrations)
        {
            var command = new SlashCommandBuilder();
            command.WithName(commandRegistration.CommandName.ToLower());
            command.WithDescription(commandRegistration.Description);
            if (commandRegistration.Options.Any())
            {
                foreach (var commandRegistrationOption in commandRegistration.Options)
                {
                    command.AddOption(commandRegistrationOption.Name.ToLower(), commandRegistrationOption.Type,
                        commandRegistrationOption.Description, commandRegistrationOption.IsRequired);
                }
            }

            _logger.Information("Registering {CommandName}", commandRegistration.CommandType.Name);
            try
            {

                applicationCommandProperties.Add(command.Build());
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message + " on " + command.Name);
            }
        }

        try
        {

            await client.BulkOverwriteGlobalApplicationCommandsAsync(applicationCommandProperties.ToArray(),
                new RequestOptions
                {
                    AuditLogReason = "Rebuild commands"
                });
            _logger.Information("Registered {Count} commands", applicationCommandProperties.Count);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
        }
    }
}