using FluentValidation;
using Quoter.Commands.Features.DeleteQuoteKeyword;
using Quoter.Commands.Features.QuoteThatKeyword;
using Quoter.Domain.Models;

namespace Quoter.Validators;

public class QuoteValidator : AbstractValidator<QuoteThatKeywordCommand>
{
    public QuoteValidator()
    {
        RuleFor(x => x.Quote)
            .NotEmpty()
            .WithMessage("Keyword cannot be empty");

        RuleFor(x => x.Quote)
            .NotEmpty()
            .WithMessage("Quote cannot be empty");

        RuleFor(x => x.Quote).NotEmpty().Must(x => !(x!.Contains("<@"))).WithMessage("Mentions are not allowed");
    }
}

public class QuoteValidator2 : AbstractValidator<DeleteQuoteKeywordCommand>
{
    public QuoteValidator2()
    {

    }
}