namespace MoqExtensions.Tests;

public interface IFooService
{
    string? DoFoo(string? a, int? b, Guid? id);

    Task<string?> DoFooAsync(string? a, int? b, Guid? id, CancellationToken cancellationToken);
}