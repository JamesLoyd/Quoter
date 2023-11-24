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
        foreach (var commandRegistration in _commandRegistrations)
        {
            var command = new SlashCommandBuilder();
            command.WithName(commandRegistration.Name);
            command.WithDescription(commandRegistration.Description);
            foreach (var commandRegistrationOption in commandRegistration.Options)
            {
                command.AddOption(commandRegistrationOption.Name, commandRegistrationOption.Type,
                    commandRegistrationOption.Description, commandRegistrationOption.IsRequired);
            }
            _logger.Information("Registering {CommandName}", commandRegistration.Name);
            applicationCommandProperties.Add(command.Build());
        }

        await client.BulkOverwriteGlobalApplicationCommandsAsync(applicationCommandProperties.ToArray(),
            new RequestOptions
            {
                AuditLogReason = "Rebuild commands"
            });
        _logger.Information("Registered {Count} commands", applicationCommandProperties.Count);
        
    }
}