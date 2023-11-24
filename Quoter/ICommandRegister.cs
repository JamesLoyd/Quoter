using Discord;
using Discord.WebSocket;
using Quoter.Commands.Abstractions;

namespace Quoter;

public interface ICommandRegister
{
    Task RegisterCommandsAsync(DiscordSocketClient client);
}

public class CommandRegister : ICommandRegister
{
    private readonly IEnumerable<ICommandRegistration> _commandRegistrations;

    public CommandRegister(IEnumerable<ICommandRegistration> commandRegistrations)
    {
        _commandRegistrations = commandRegistrations;
    }

    public async Task RegisterCommandsAsync(DiscordSocketClient client)
    {
        List<ApplicationCommandProperties> applicationCommandProperties = new();
        foreach (ICommandRegistration commandRegistration in _commandRegistrations)
        {
            var command = new SlashCommandBuilder();
            command.WithName(commandRegistration.Name);
            command.WithDescription(commandRegistration.Description);
            foreach (var commandRegistrationOption in commandRegistration.Options)
            {
                command.AddOption(commandRegistrationOption.Name, commandRegistrationOption.Type,
                    commandRegistrationOption.Description, commandRegistrationOption.IsRequired);
            }

            applicationCommandProperties.Add(command.Build());
        }

        await client.BulkOverwriteGlobalApplicationCommandsAsync(applicationCommandProperties.ToArray(),
            new RequestOptions
            {
                AuditLogReason = "Rebuild commands"
            });
    }
}