using Discord;
using Discord.WebSocket;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quoter.Commands.Features.QuoteThatKeyword;
using Quoter.Domain.Models;
using Quoter.Entities;

namespace Quoter;

public interface IBot
{
    Task MainAsync();
}

public class Bot : IBot
{
    private DiscordSocketClient _client;
    private QuoterContext _quoterContext;
    private IMediator _mediator;
    private readonly ICommandDispatcher _commandDispatcher;
    private ICommandRegister _commandRegister;

    public Bot(QuoterContext quoterContext, ICommandRegister commandRegister, IMediator mediator, ICommandDispatcher commandDispatcher)
    {
        _quoterContext = quoterContext;
        _commandRegister = commandRegister;
        _mediator = mediator;
        _commandDispatcher = commandDispatcher;
    }

    public async Task MainAsync()
    {
        DiscordSocketConfig config = new()
        {
            UseInteractionSnowflakeDate = false
        };
        _client = new DiscordSocketClient(config);

        //  You can assign your bot token to a string, and pass that in to connect.
        //  This is, however, insecure, particularly if you plan to have your code hosted in a public repository.
        var token = "MTE3NDE2NTcxMjU1ODg4Mjg4Ng.GLKDdK.WF0dOeyE-D-hp3JIVfrRF8a1qRuAmRLjCp2kaM"; //prod == "MTE3MTg5ODc4MjM1OTc2MDkyNg.G11025.0VzY4Zz9kKGDWkUWv39vkcAZgcKxRgejuMsrJQ";

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();
        _client.Ready += Client_Ready;
        _client.SlashCommandExecuted += SlashCommandHandler;
        // Block this task until the program is closed.
    }

    // Let's build a guild command! We're going to need a guild so lets just put that in a variable.
    public async Task Client_Ready()
    {
        Console.WriteLine("client ready");

        await _commandRegister.RegisterCommandsAsync(_client);
        // Let's do our global command
        // var quoteThatCommand = new SlashCommandBuilder();
        // quoteThatCommand.WithName("quote-that");
        // quoteThatCommand.WithDescription("I help you quopte");
        // quoteThatCommand.AddOption("quoted", ApplicationCommandOptionType.User, "User to quote", true);
        //
        //
        // var requestQuoteCommand = new SlashCommandBuilder();
        // requestQuoteCommand.WithName("rq-that");
        // requestQuoteCommand.WithDescription("I help you get quotes");
        // requestQuoteCommand.AddOption("quoted", ApplicationCommandOptionType.User, "User to re-quote", true);
        //
        // var requestQuoteKeywordCommand = new SlashCommandBuilder();
        // requestQuoteKeywordCommand.WithName("rq-k");
        // requestQuoteKeywordCommand.WithDescription("I help you get quotes");
        // requestQuoteKeywordCommand.AddOption("keyword", ApplicationCommandOptionType.String, "User to re-quote", true);
        //
        //
        // var listQuotesCommand = new SlashCommandBuilder();
        // listQuotesCommand.WithName("list-quotes");
        // listQuotesCommand.WithDescription("I help you get quotes");
        // listQuotesCommand.AddOption("quoted", ApplicationCommandOptionType.User, "User to re-quote", true);
        //
        // var purgeQuotesCommand = new SlashCommandBuilder();
        // purgeQuotesCommand.WithName("purge-quotes");
        // purgeQuotesCommand.WithDescription("I help you get quotes");
        // purgeQuotesCommand.AddOption("quoted", ApplicationCommandOptionType.User, "User to re-quote", true);
        //
        // var purgeQuoteCommand = new SlashCommandBuilder();
        // purgeQuoteCommand.WithName("purge-quote");
        // purgeQuoteCommand.WithDescription("I help you get quotes");
        // purgeQuoteCommand.AddOption("quoted", ApplicationCommandOptionType.User, "User to re-quote", true);
        // purgeQuoteCommand.AddOption("quote-id", ApplicationCommandOptionType.Number, "User to re-quote", true);
        //
        //
        // var addPerm = new SlashCommandBuilder();
        // addPerm.WithName("add-perm");
        // addPerm.WithDescription("I help you get quotes");
        // addPerm.AddOption("permission", ApplicationCommandOptionType.Boolean, "Can purge quotes", true);
        // addPerm.AddOption("quoted", ApplicationCommandOptionType.Role, "User to re-quote", true);
        //
        // var deleteKeywordQuoteCommand = new SlashCommandBuilder();
        // deleteKeywordQuoteCommand.WithName("delete-quote-keyword");
        // deleteKeywordQuoteCommand.WithDescription("I help you get quotes");
        // deleteKeywordQuoteCommand.AddOption("keyword", ApplicationCommandOptionType.String, "User to re-quote", true);
        //
        // var listKeywordsQuoteCommand = new SlashCommandBuilder();
        // listKeywordsQuoteCommand.WithName("list-keywords");
        // listKeywordsQuoteCommand.WithDescription("I help you get quotes");
        //
        // List<ApplicationCommandProperties> applicationCommandProperties = new();


        try
        {
            // Now that we have our builder, we can call the CreateApplicationCommandAsync method to make our slash command.
            // With global commands we don't need the guild.
            // applicationCommandProperties.Add(quoteThatCommand.Build());
            // applicationCommandProperties.Add(requestQuoteCommand.Build());
            // applicationCommandProperties.Add(listQuotesCommand.Build());
            // applicationCommandProperties.Add(purgeQuotesCommand.Build());
            // applicationCommandProperties.Add(purgeQuoteCommand.Build());
            // applicationCommandProperties.Add(addPerm.Build());
            // applicationCommandProperties.Add(requestQuoteKeywordCommand.Build());
            // applicationCommandProperties.Add(deleteKeywordQuoteCommand.Build());
            // applicationCommandProperties.Add(listKeywordsQuoteCommand.Build());
            //
            //
            // await _client.BulkOverwriteGlobalApplicationCommandsAsync(applicationCommandProperties.ToArray(),
            //     new RequestOptions
            //     {
            //         AuditLogReason = "Rebuild commands"
            //     });

            // Using the ready event is a simple implementation for the sake of the example. Suitable for testing and development.
            // For a production bot, it is recommended to only run the CreateGlobalApplicationCommandAsync() once for each command.
        }
        catch (Exception exception)
        {
            // If our command was invalid, we should catch an ApplicationCommandException. This exception contains the path of the error as well as the error message. You can serialize the Error field in the exception to get a visual of where your error is.
            var json = JsonConvert.SerializeObject(exception, Formatting.Indented);

            // You can send this error somewhere or just print it to the console, for this example we're just going to print it.
            Console.WriteLine(json);
        }
    }

    private async Task SlashCommandHandler(SocketSlashCommand command)
    {
        var response = await _commandDispatcher.DispatchCommand(command);
        await command.RespondAsync(response.Message, ephemeral: response.Ephemeral);
        if (command.Data.Name == "quote-that")
        {
            Console.WriteLine("executing start");
            var id = command.Data.Options.ElementAt(0).Value! as IUser;
            Console.WriteLine("id is " + id);
            if ((id).Username == "quoter")
            {
                await command.RespondAsync("Can't quote myself", ephemeral: true);
                return;
            }

            var userId = await command.Channel.GetUsersAsync()
                .Where(x => x.Any(y => y.Username == id.Username.ToString())).ToListAsync();
            try
            {
                var hello = userId.SelectMany(x => x).Select(x => new { x.Username, x.Id });
                Console.WriteLine(JsonConvert.SerializeObject(hello));
                var j = hello.FirstOrDefault(x => x.Username == id.Username.ToString());
                Console.WriteLine(JsonConvert.SerializeObject(j));
                var c = (long)j.Id;
                var d = (ulong)c;
                Console.WriteLine("Done casting" + d);
                var messages2 = await command.Channel.GetMessagesAsync().ToListAsync();
                Console.WriteLine("I have messages?" + messages2.Any());


                var test = messages2.SelectMany(x => x)
                    .Select(x => new
                    {
                        content = x.Content, username = x.Author.Username, mentioned = x.MentionedUserIds,
                        attchments = x.Attachments
                    }).Reverse().LastOrDefault(x => x.username == id.Username.ToString());
                Console.WriteLine("t5est os " + JsonConvert.SerializeObject(test));
                Console.WriteLine("I got this far");
                if (test == null)
                {
                    await command.RespondAsync($"Can't quote this", ephemeral: true);
                    return;
                }

                if (test.content == "no quote")
                {
                    await command.RespondAsync($"Can't quote <@{d}>");
                    return;
                }

                if (test.mentioned?.Any() ?? false)
                {
                    await command.RespondAsync("I can't quote messages with mentions", ephemeral: true);
                    return;
                }

                if (test.attchments?.Any() ?? false)
                {
                    await command.RespondAsync("I can't quote  messages with attachments", ephemeral: true);
                    return;
                }

                if (string.IsNullOrWhiteSpace(test.content) || string.IsNullOrEmpty(test.content))
                {
                    await command.RespondAsync("I can't quote empty messages", ephemeral: true);
                    return;
                }

                await command.DeferAsync();

                Console.WriteLine("content is" + test.content);

                if (_quoterContext.QuoteUserRecords.Count() > 100)
                {
                    _quoterContext.QuoteUserRecords.Remove(_quoterContext.QuoteUserRecords.OrderBy(x => x.Id).Last());
                    await _quoterContext.SaveChangesAsync();
                }

                var guild = _client.GetGuild(command.GuildId.Value);
                var gus = guild.Users.FirstOrDefault(x => x.Id == id.Id);
                var res1 = await _quoterContext.QuoteUserRecords.AddAsync(new QuoteRecord
                {
                    UserName = id.Username,
                    GlobalName = gus?.Nickname ?? id.GlobalName,
                    UserId = d.ToString(),
                    ChannelId = command.Channel.Id.ToString(),
                    ChannelName = command.Channel.Name,
                    GuildId = command.GuildId.ToString(),
                    Text = test.content
                });
                await _quoterContext.SaveChangesAsync();
                await command.ModifyOriginalResponseAsync(x =>
                    x.Content = $"I quoted <@{d}> with {test.content}, uniqueId = {res1.Entity.Id}");
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        if (command.Data.Name == "rq-that")
        {
            await command.DeferAsync();
            var id = command.Data.Options.ElementAt(0).Value! as IUser;
            try
            {
                var random = new Random();
                var randomNumber = random.Next(0,
                    _quoterContext.QuoteUserRecords.Count(x =>
                        x.UserId == id.Id.ToString() && x.GuildId == command.GuildId.ToString()));
                var messagea = _quoterContext.QuoteUserRecords.Where(
                    x => x.UserId == id.Id.ToString() && x.GuildId == command.GuildId.ToString()
                );
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(messagea));
                var message = messagea.ToArray()[randomNumber];
                await command.ModifyOriginalResponseAsync(x => x.Content = $"{message.GlobalName}: {message.Text}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(ex));
                await command.ModifyOriginalResponseAsync(x =>
                {
                    x.Content = "There was an error";
                    x.Flags = MessageFlags.Ephemeral;
                });
            }
        }

        if (command.Data.Name == "list-quotes")
        {
            var id22 = command.Data.Options.ElementAt(0).Value! as IUser;
            var guild = _client.GetGuild(command.GuildId.Value).GetUser(id22.Id);
            var perms = await _quoterContext.Permissions.ToListAsync();
            foreach (var perm in perms)
            {
                var role = guild.Roles.FirstOrDefault(x => x.Id.ToString() == perm.RoleId);
                if (role == null) continue;
                if (perm.CanPurge)
                {
                    var id = command.Data.Options.ElementAt(0).Value! as IUser;
                    var message = _quoterContext.QuoteUserRecords.Where(x => x.UserName == id.Username);

                    await command.RespondAsync(
                        "Message count for user is: " +
                        string.Join(',', message.Select(x => x.Text + "with id " + x.Id)),
                        ephemeral: true);
                }
            }
        }

        if (command.Data.Name == "purge-quotes")
        {
            var purged = false;
            var id = command.Data.Options.ElementAt(0).Value! as IUser;
            var guild = _client.GetGuild(command.GuildId.Value).GetUser(command.User.Id);
            var perms = await _quoterContext.Permissions.ToListAsync();
            foreach (var perm in perms)
            {
                var role = guild.Roles.FirstOrDefault(x => x.Id.ToString() == perm.RoleId);
                if (role == null) continue;
                if (perm.CanPurge)
                {
                    _quoterContext.QuoteUserRecords.RemoveRange(_quoterContext.QuoteUserRecords.Where(x =>
                        x.UserName == id.Username && x.GuildId == command.GuildId.ToString()));
                    _quoterContext.SaveChanges();
                    await command.RespondAsync("Purged quotes for user " + id.Username);
                    purged = true;
                }
            }

            if (!purged)
            {
                await command.RespondAsync("Failure to purge");
            }
        }

        if (command.Data.Name == "purge-quote")
        {
            try
            {
                var purged = false;
                var id = command.Data.Options.ElementAt(0).Value! as IUser;
                var guild = _client.GetGuild(command.GuildId.Value).GetUser(command.User.Id);
                var perms = await _quoterContext.Permissions.ToListAsync();
                await command.DeferAsync();
                foreach (var perm in perms)
                {
                    var role = guild.Roles.FirstOrDefault(x => x.Id.ToString() == perm.RoleId);
                    if (role == null) continue;
                    Console.WriteLine("Role is" + role.Name);
                    if (perm.CanPurge)
                    {
                        _quoterContext.QuoteUserRecords.RemoveRange(_quoterContext.QuoteUserRecords.Where(x =>
                            x.UserName == id.Username && x.GuildId == command.GuildId.ToString() &&
                            x.Id.ToString() == command.Data.Options.ElementAt(1).Value.ToString()));
                        _quoterContext.SaveChanges();
                        await command.ModifyOriginalResponseAsync(x =>
                            x.Content = "Purged quotes for user " + id.Username);
                        purged = true;
                    }
                }

                if (!purged)
                {
                    await command.ModifyOriginalResponseAsync(x => x.Content = "Failure to purge");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(ex));
                await command.ModifyOriginalResponseAsync(x => x.Content = "There was an error purging");
            }
        }

        if (command.Data.Name == "add-perm")
        {
            if (command.User.Username != "jrocflanders")
            {
                await command.RespondAsync("Only my creator can add roles for now", ephemeral: true);
                return;
            }

            var id = command.Data.Options.ElementAt(1).Value! as IRole;
            _quoterContext.Permissions.Add(new Permission
            {
                Id = Guid.NewGuid(),
                RoleId = id.Id.ToString(),
                RoleName = id.Name,
                CanPurge = true
            });
            _quoterContext.SaveChanges();
            await command.RespondAsync("Add role " + id.Name);
        }

        if (command.Data.Name == "rq-k")
        {
            var id = command.Data.Options.ElementAt(0).Value! as string;
            try
            {
                var messagea = _quoterContext.Quotes.Where(
                    x => x.KeyWord == id && x.GuildId == command.GuildId.ToString()
                );
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(messagea));
                var message = messagea.FirstOrDefault();
                await command.RespondAsync($"{message.Text}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(ex));
                await command.RespondAsync("There was an error", ephemeral: true);
            }
        }

        if (command.Data.Name == "delete-quote-keyword")
        {
            var purged = false;
            var user = command.User.Id;
            var id = command.Data.Options.ElementAt(0).Value! as string;
            var guild = _client.GetGuild(command.GuildId.Value).GetUser(user);
            var perms = await _quoterContext.Permissions.ToListAsync();
            foreach (var perm in perms)
            {
                var role = guild.Roles.FirstOrDefault(x => x.Id.ToString() == perm.RoleId);
                if (role == null) continue;
                if (perm.CanPurge)
                {
                    _quoterContext.Quotes.RemoveRange(_quoterContext.Quotes.Where(x =>
                        x.KeyWord == id && x.GuildId == command.GuildId.ToString()));
                    _quoterContext.SaveChanges();
                    await command.RespondAsync("Purged quote with keyword " + id);
                    purged = true;
                }
            }

            if (!purged)
            {
                await command.RespondAsync("Failure to purge");
            }
        }

        if (command.CommandName == "list-keywords")
        {
            await command.DeferAsync(ephemeral: true);
            try
            {
                var keywords = await _quoterContext.Quotes.ToListAsync();

                var keywords2 = keywords.Where(x => x.GuildId.ToString() == command.GuildId.ToString())
                    .Select(x => x.KeyWord).ToList();
                await command.ModifyOriginalResponseAsync(
                    x => x.Content = "Keywords are: " + string.Join(',', keywords2));
            }
            catch (Exception ex)
            {
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(ex));
                await command.ModifyOriginalResponseAsync(x => x.Content = "There was an error");
            }
        }
    }
}