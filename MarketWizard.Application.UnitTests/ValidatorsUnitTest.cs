using MarketWizard.Application.Features.Watchlists.Commands;
using MarketWizard.Application.Validators;
using MarketWizard.Domain.Entities;

namespace MarketWizard.Application.UnitTests;

public class ValidatorsUnitTest
{
    [Fact]
    public void CreateWatchlistCommandValidator_WhenDtoNull_IsValid()
    {
        var validator = new CreateWatchlistCommandValidator();
        var command = new CreateWatchlistCommand(null);

        var result = validator.Validate(command);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void CreateWatchlistCommandValidator_WhenNameTooLong_IsInvalid()
    {
        var validator = new CreateWatchlistCommandValidator();
        var command = new CreateWatchlistCommand(new CreateWatchlistDto(new string('a', 101)));

        var result = validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName.Contains("Name"));
    }

    [Fact]
    public void UpdateWatchlistCommandValidator_WhenIdInvalidGuid_IsInvalid()
    {
        var validator = new UpdateWatchlistCommandValidator();
        var command = new UpdateWatchlistCommand("not-a-guid", new UpdateWatchlistDto("Valid Name"));

        var result = validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Id");
    }

    [Fact]
    public void UpdateWatchlistCommandValidator_WhenNameEmpty_IsInvalid()
    {
        var validator = new UpdateWatchlistCommandValidator();
        var command = new UpdateWatchlistCommand(Guid.NewGuid().ToString(), new UpdateWatchlistDto(""));

        var result = validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName.Contains("Name"));
    }

    [Fact]
    public void UpdateWatchlistCommandValidator_WhenValid_IsValid()
    {
        var validator = new UpdateWatchlistCommandValidator();
        var command = new UpdateWatchlistCommand(Guid.NewGuid().ToString(), new UpdateWatchlistDto("Valid Name"));

        var result = validator.Validate(command);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void DeleteWatchlistCommandValidator_WhenIdEmptyOrNotGuid_IsInvalid()
    {
        var validator = new DeleteWatchlistCommandValidator();

        var emptyResult = validator.Validate(new DeleteWatchlistCommand(""));
        var invalidResult = validator.Validate(new DeleteWatchlistCommand("invalid"));

        Assert.False(emptyResult.IsValid);
        Assert.False(invalidResult.IsValid);
    }

    [Fact]
    public void CreateWatchlistItemCommandValidator_WhenTickerEmpty_IsInvalid()
    {
        var validator = new CreateWatchlistItemCommandValidator();
        var command = new CreateWatchlistItemCommand(new WatchlistItemDto(Guid.NewGuid().ToString(), ""));

        var result = validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName.Contains("Ticker"));
    }

    [Fact]
    public void CreateWatchlistItemCommandValidator_WhenValid_IsValid()
    {
        var validator = new CreateWatchlistItemCommandValidator();
        var command = new CreateWatchlistItemCommand(new WatchlistItemDto(Guid.NewGuid().ToString(), "AAPL"));

        var result = validator.Validate(command);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void DeleteWatchlistItemCommandValidator_WhenValid_IsValid()
    {
        var validator = new DeleteWatchlistItemCommandValidator();
        var command = new DeleteWatchlistItemCommand(new WatchlistItemDto(Guid.NewGuid().ToString(), "AAPL"));

        var result = validator.Validate(command);

        Assert.True(result.IsValid);
    }
}
