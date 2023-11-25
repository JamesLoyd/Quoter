using Discord.WebSocket;

namespace Quoter.Domain.Models;

public class GuildModel
{
    public ulong Id { get; set; }
    public List<ulong> RoleIds { get; set; }
}