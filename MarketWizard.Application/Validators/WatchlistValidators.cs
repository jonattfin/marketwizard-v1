using FluentValidation;
using MarketWizard.Application.Features.Watchlists.Commands;
using MarketWizard.Domain.Entities;

namespace MarketWizard.Application.Validators;

public class CreateWatchlistDtoValidator : AbstractValidator<CreateWatchlistDto>
{
    public CreateWatchlistDtoValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(100).WithMessage("Watchlist name must not exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.Name));
    }
}

public class UpdateWatchlistDtoValidator : AbstractValidator<UpdateWatchlistDto>
{
    public UpdateWatchlistDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Watchlist name is required.")
            .MaximumLength(100).WithMessage("Watchlist name must not exceed 100 characters.");
    }
}

public class WatchlistItemDtoValidator : AbstractValidator<WatchlistItemDto>
{
    public WatchlistItemDtoValidator()
    {
        RuleFor(x => x.Ticker)
            .NotEmpty().WithMessage("Ticker is required.")
            .MaximumLength(20).WithMessage("Ticker must not exceed 20 characters.")
            .When(x => string.IsNullOrEmpty(x.ItemId));

        RuleFor(x => x.WatchlistId ?? x.Id)
            .NotEmpty().WithMessage("Watchlist ID is required.")
            .Must(BeValidGuid).WithMessage("Watchlist ID must be a valid GUID.");
    }

    private static bool BeValidGuid(string? id) => Guid.TryParse(id, out _);
}

public class CreateWatchlistCommandValidator : AbstractValidator<CreateWatchlistCommand>
{
    public CreateWatchlistCommandValidator()
    {
        When(x => x.Dto != null, () =>
        {
            RuleFor(x => x.Dto!).SetValidator(new CreateWatchlistDtoValidator());
        });
    }
}

public class UpdateWatchlistCommandValidator : AbstractValidator<UpdateWatchlistCommand>
{
    public UpdateWatchlistCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Watchlist ID is required.")
            .Must(id => Guid.TryParse(id, out _)).WithMessage("Watchlist ID must be a valid GUID.");

        RuleFor(x => x.Dto)
            .NotNull().WithMessage("Update data is required.")
            .SetValidator(new UpdateWatchlistDtoValidator());
    }
}

public class DeleteWatchlistCommandValidator : AbstractValidator<DeleteWatchlistCommand>
{
    public DeleteWatchlistCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Watchlist ID is required.")
            .Must(id => Guid.TryParse(id, out _)).WithMessage("Watchlist ID must be a valid GUID.");
    }
}

public class CreateWatchlistItemCommandValidator : AbstractValidator<CreateWatchlistItemCommand>
{
    public CreateWatchlistItemCommandValidator()
    {
        RuleFor(x => x.Dto)
            .NotNull().WithMessage("Item data is required.")
            .SetValidator(new WatchlistItemDtoValidator());
    }
}

public class DeleteWatchlistItemCommandValidator : AbstractValidator<DeleteWatchlistItemCommand>
{
    public DeleteWatchlistItemCommandValidator()
    {
        RuleFor(x => x.Dto)
            .NotNull().WithMessage("Item data is required.");

        RuleFor(x => x.Dto.WatchlistId ?? x.Dto.Id)
            .NotEmpty().WithMessage("Watchlist ID is required.")
            .Must(id => Guid.TryParse(id, out _)).WithMessage("Watchlist ID must be a valid GUID.")
            .When(x => x.Dto != null);

        RuleFor(x => x.Dto.ItemId ?? x.Dto.Ticker)
            .NotEmpty().WithMessage("Item ID or ticker is required.")
            .When(x => x.Dto != null);
    }
}
