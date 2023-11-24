using Quoter.Domain.Models;

namespace Quoter.Domain.Interfaces;

public interface IHasUser
{
    public UserModel User { get; set; }
}