namespace MoqExtensions.Tests;

public class FooService : IFooService
{
    public string? DoFoo(
        string? a,
        int? b,
        Guid? id)
        => default;

    public Task<string?> DoFooAsync(
        string? a,
        int? b,
        Guid? id,
        CancellationToken cancellationToken)
        => Task.FromResult<string?>(default);
}