using MediatR;

namespace Customers.Consumer.Messages;

public class CustomerCreated : ISqsMessage
{
    public required string GitHubUsername { get; init; } = default!;

    public required string FullName { get; init; } = default!;

    public required string Email { get; init; } = default!;

    public required DateTime DateOfBirth { get; init; } = default!;
}

public class CustomerUpdated : ISqsMessage
{
    public required Guid Id { get; set; }
    
    public required string GitHubUsername { get; init; } = default!;

    public required string FullName { get; init; } = default!;

    public required string Email { get; init; } = default!;

    public required DateTime DateOfBirth { get; init; } = default!;
}

public class CustomerDeleted : ISqsMessage
{
    public required Guid Id { get; set; }
}