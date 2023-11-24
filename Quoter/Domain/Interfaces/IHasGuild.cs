using Quoter.Domain.Models;

namespace Quoter.Domain.Interfaces;

public interface IHasGuild
{
    public GuildModel Guild { get; set; }
}