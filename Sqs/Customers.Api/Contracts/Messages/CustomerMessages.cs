namespace Customers.Api.Contracts.Messages;

public class CustomerCreated
{
    public required string GitHubUsername { get; init; } = default!;

    public required string FullName { get; init; } = default!;

    public required string Email { get; init; } = default!;

    public required DateTime DateOfBirth { get; init; } = default!;
}

public class CustomerUpdated
{
    public required Guid Id { get; set; }
    
    public required string GitHubUsername { get; init; } = default!;

    public required string FullName { get; init; } = default!;

    public required string Email { get; init; } = default!;

    public required DateTime DateOfBirth { get; init; } = default!;
}

public class CustomerDeleted
{
    public required Guid Id { get; set; }
}