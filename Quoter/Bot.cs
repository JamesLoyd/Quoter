using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
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

    public Bot(QuoterContext quoterContext)
    {
        _quoterContext = quoterContext;
    }
    public async Task MainAsync()
    {
        _client = new DiscordSocketClient();

        //  You can assign your bot token to a string, and pass that in to connect.
        //  This is, however, insecure, particularly if you plan to have your code hosted in a public repository.
        var token = "MTE3MTg5ODc4MjM1OTc2MDkyNg.GFZsex.370INlou9pvr-qHs1xZdtU27J_Tp0xVRgCtS0w";
        
        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();
        _client.Ready += Client_Ready;
        _client.SlashCommandExecuted += SlashCommandHandler;
        // Block this task until the program is closed.
        await Task.Delay(-1);
    }
    
    // Let's build a guild command! We're going to need a guild so lets just put that in a variable.
    public async Task Client_Ready()
    {
        Console.WriteLine("client ready");
        // Let's do our global command
        var quoteThatCommand = new SlashCommandBuilder();
        quoteThatCommand.WithName("quote-that");
        quoteThatCommand.WithDescription("I help you quopte");
        quoteThatCommand.AddOption("quoted", ApplicationCommandOptionType.User, "User to quote", true);
   
        var requestQuoteCommand = new SlashCommandBuilder();
        requestQuoteCommand.WithName("rq-that");
        requestQuoteCommand.WithDescription("I help you get quotes");
        requestQuoteCommand.AddOption("quoted", ApplicationCommandOptionType.User, "User to re-quote", true);

        var listQuotesCommand = new SlashCommandBuilder();
        listQuotesCommand.WithName("list-quotes");
        listQuotesCommand.WithDescription("I help you get quotes");
        listQuotesCommand.AddOption("quoted", ApplicationCommandOptionType.User, "User to re-quote", true);
        
        var purgeQuoteCommand = new SlashCommandBuilder();
        purgeQuoteCommand.WithName("purge-quotes");
        purgeQuoteCommand.WithDescription("I help you get quotes");
        purgeQuoteCommand.AddOption("quoted", ApplicationCommandOptionType.User, "User to re-quote", true);

        var addPerm = new SlashCommandBuilder();
        addPerm.WithName("add-perm");
        addPerm.WithDescription("I help you get quotes");
        addPerm.AddOption("quoted", ApplicationCommandOptionType.Role, "User to re-quote", true);

        List<ApplicationCommandProperties> applicationCommandProperties = new();


        try
        {
            // Now that we have our builder, we can call the CreateApplicationCommandAsync method to make our slash command.
            // With global commands we don't need the guild.
            applicationCommandProperties.Add(quoteThatCommand.Build());
            applicationCommandProperties.Add(requestQuoteCommand.Build());
            applicationCommandProperties.Add(listQuotesCommand.Build());
            applicationCommandProperties.Add(purgeQuoteCommand.Build());
            applicationCommandProperties.Add(addPerm.Build());

            await _client.BulkOverwriteGlobalApplicationCommandsAsync(applicationCommandProperties.ToArray(), new RequestOptions
            {
                AuditLogReason = "Rebuild commands"
            });

            // Using the ready event is a simple implementation for the sake of the example. Suitable for testing and development.
            // For a production bot, it is recommended to only run the CreateGlobalApplicationCommandAsync() once for each command.
        }
        catch(Exception exception)
        {
            // If our command was invalid, we should catch an ApplicationCommandException. This exception contains the path of the error as well as the error message. You can serialize the Error field in the exception to get a visual of where your error is.
            var json = JsonConvert.SerializeObject(exception, Formatting.Indented);

            // You can send this error somewhere or just print it to the console, for this example we're just going to print it.
            Console.WriteLine(json);
        }
    }
    
    private async Task SlashCommandHandler(SocketSlashCommand command)
    {
        if (command.User.Username != "jrocflanders")
        {
            await command.RespondAsync("Not yet fam", ephemeral :true);
            return;
        }
        
        if (command.Data.Name == "quote-that")
        {
            
            Console.WriteLine("executing start");
            var id = command.Data.Options.ElementAt(0).Value! as IUser;
            Console.WriteLine("id is " + id);
            if ((id).Username == "quoter")
            {
                await command.RespondAsync("Can't quote myself", ephemeral :true);
                return;
            }

            var userId = await command.Channel.GetUsersAsync().Where(x => x.Any(y => y.Username == id.Username.ToString())).ToListAsync();
            try
            {
                var hello = userId.SelectMany(x => x).Select(x => new {x.Username, x.Id});
                Console.WriteLine(JsonConvert.SerializeObject(hello));
                var j = hello.FirstOrDefault(x => x.Username == id.ToString());
                Console.WriteLine(JsonConvert.SerializeObject(j));
                var c = (long)j.Id;
            var d = (ulong)c;
            Console.WriteLine("Done casting" + d);
            var messages2 = await command.Channel.GetMessagesAsync().ToListAsync();
            Console.WriteLine("I have messages?" + messages2.Any());

            
var test =messages2.SelectMany(x => x).Select(x => new {content = x.Content, username = x.Author.Username, mentioned = x.MentionedUserIds }).Reverse().LastOrDefault(x => x.username == id.ToString());
Console.WriteLine("t5est os " + JsonConvert.SerializeObject(test));
            Console.WriteLine("I got this far");
            if (test.content == "no quote")
            {
                await command.RespondAsync($"Can't quote <@{d}>");
                return;                
            }

            if (test.mentioned.Any())
            {
                await command.RespondAsync("I can't quote messages with mentions", ephemeral: true);
                return;
            }
               await command.RespondAsync($"I quoted <@{d}> with {test.content}");

               if (_quoterContext.QuoteRecords.Count() > 4)
               {
                   _quoterContext.QuoteRecords.Remove(_quoterContext.QuoteRecords.Last());
                   _quoterContext.SaveChanges();
               }
               var guild = _client.GetGuild(command.GuildId.Value);
               var gus = guild.Users.FirstOrDefault(x => x.Id == id.Id);
               _quoterContext.QuoteRecords.Add(new QuoteRecord
               {
                   Id = Guid.NewGuid(),
                   UserName = id.Username,
                   GlobalName = gus?.Nickname ?? id.GlobalName,
                   UserId = d.ToString(),
                   ChannelId = command.Channel.Id.ToString(),
                   ChannelName = command.Channel.Name,
                   GuildId = command.GuildId.ToString(),
                   Text = test.content
               });
               _quoterContext.SaveChanges();
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        if (command.Data.Name == "rq-that")
        {
            var id = command.Data.Options.ElementAt(0).Value! as IUser;
            try
            {

                var random = new Random();
                var randomNumber = random.Next(0,
                    _quoterContext.QuoteRecords.Count(x =>
                        x.UserId == id.Id.ToString() && x.GuildId == command.GuildId.ToString()));
                var messagea = _quoterContext.QuoteRecords.Where(
                    x => x.UserId == id.Id.ToString() && x.GuildId == command.GuildId.ToString()
                );
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(messagea));
                var message =messagea.ToArray()[randomNumber];
                await command.RespondAsync($"{message.GlobalName}: {message.Text}");
            }
            catch(Exception ex)
            {
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(ex));
                await command.RespondAsync("There was an error", ephemeral :true);
            }
        }
        
        if (command.Data.Name == "list-quotes")
        {
            var id = command.Data.Options.ElementAt(0).Value! as IUser;
            var message = _quoterContext.QuoteRecords.Where(x => x.UserName == id.Username);
            await command.RespondAsync("Message count for user is: " + string.Join(',',message.Select(x => x.Text)), ephemeral: true);
        }
        
        if(command.Data.Name == "purge-quotes")
        {
            var id = command.Data.Options.ElementAt(0).Value! as IUser;
             _quoterContext.QuoteRecords.RemoveRange(_quoterContext.QuoteRecords.Where( x => x.UserName == id.Username && x.GuildId == command.GuildId.ToString()));
             _quoterContext.SaveChanges();
            await command.RespondAsync("Purged quotes for user " + id.Username);
        }

        if (command.Data.Name == "add-perm")
        {
            var id = command.Data.Options.ElementAt(0).Value! as IRole;
           _quoterContext.Permissions.Add(new Permission
           {
               Id = Guid.NewGuid(),
               RoleId = id.Id.ToString(),
               RoleName = id.Name,
           });
           _quoterContext.SaveChanges();
            await command.RespondAsync("Add role " + id.Name);
        }
    }
}