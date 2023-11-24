using Quoter.Domain.Models;

namespace Quoter.Domain.Interfaces;

public interface IHasChannel
{
    public ChannelModel Channel { get; set; }
}