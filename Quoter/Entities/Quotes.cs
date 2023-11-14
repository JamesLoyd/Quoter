namespace Quoter.Entities;

public class Quotes
{
    public long Id { get; set; }
    public string KeyWord { get; set; } = "";
    public string GlobalName { get; set; } = "";
    public string UserName { get; set; } = "";
    public string UserId { get; set; } = "";
    public string Text { get; set; } = "";
    public string ChannelName { get; set; } = "";
    public string ChannelId { get; set; } = "";
    public string GuildId { get; set; } = "";
}