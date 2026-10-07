using FluentValidation;
using FluentValidation.Results;
using MarketWizard.Application.Behaviors;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace MarketWizard.Application.UnitTests;

public class BehaviorsUnitTest
{
    public record SampleRequest(string Name) : IRequest<string>;

    [Fact]
    public async Task LoggingBehavior_LogsRequestAndReturnsResponse()
    {
        var logger = Substitute.For<ILogger<LoggingBehavior<SampleRequest, string>>>();
        var behavior = new LoggingBehavior<SampleRequest, string>(logger);

        var request = new SampleRequest("Test");
        var response = await behavior.Handle(request, _ => Task.FromResult("Success"), CancellationToken.None);

        Assert.Equal("Success", response);
        logger.ReceivedWithAnyArgs(2).Log(
            LogLevel.Information,
            default,
            Arg.Any<object>(),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public async Task LoggingBehavior_WhenExceptionThrown_LogsErrorAndRethrows()
    {
        var logger = Substitute.For<ILogger<LoggingBehavior<SampleRequest, string>>>();
        var behavior = new LoggingBehavior<SampleRequest, string>(logger);

        var request = new SampleRequest("Test");
        var expectedException = new InvalidOperationException("Failure");

        var actualException = await Assert.ThrowsAsync<InvalidOperationException>(
            () => behavior.Handle(request, _ => Task.FromException<string>(expectedException), CancellationToken.None));

        Assert.Same(expectedException, actualException);
        logger.Received(1).Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Any<object>(),
            expectedException,
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public async Task ValidationBehavior_WhenNoValidators_CallsNext()
    {
        var behavior = new ValidationBehavior<SampleRequest, string>([]);
        var request = new SampleRequest("Test");

        var response = await behavior.Handle(request, _ => Task.FromResult("Passed"), CancellationToken.None);

        Assert.Equal("Passed", response);
    }

    [Fact]
    public async Task ValidationBehavior_WhenValidationPasses_CallsNext()
    {
        var validator = Substitute.For<IValidator<SampleRequest>>();
        validator.ValidateAsync(Arg.Any<ValidationContext<SampleRequest>>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new ValidationResult()));

        var behavior = new ValidationBehavior<SampleRequest, string>([validator]);
        var request = new SampleRequest("Valid");

        var response = await behavior.Handle(request, _ => Task.FromResult("Passed"), CancellationToken.None);

        Assert.Equal("Passed", response);
    }

    [Fact]
    public async Task ValidationBehavior_WhenValidationFails_ThrowsValidationException()
    {
        var validator = Substitute.For<IValidator<SampleRequest>>();
        var failures = new List<ValidationFailure>
        {
            new("Name", "Name is invalid.")
        };
        validator.ValidateAsync(Arg.Any<ValidationContext<SampleRequest>>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new ValidationResult(failures)));

        var behavior = new ValidationBehavior<SampleRequest, string>([validator]);
        var request = new SampleRequest("");

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => behavior.Handle(request, _ => Task.FromResult("Should not reach"), CancellationToken.None));

        Assert.Single(ex.Errors);
        Assert.Equal("Name is invalid.", ex.Errors.First().ErrorMessage);
    }
}
