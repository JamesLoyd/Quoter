using Quoter.Commands.Abstractions;

namespace Quoter.Commands.Features.QuoteThatKeyword;

public class QuoteThatKeywordRegistration : ICommandRegistration
{
    public string Name => "QuoteThatKeyword";
}