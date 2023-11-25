using Discord.WebSocket;

namespace Quoter.Domain.Models;

public class GuildModel
{
    public ulong Id { get; set; }
    public IEnumerable<SocketRole> Roles { get; set; }
}