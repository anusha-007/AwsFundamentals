namespace SqsPublisher;

public class CustomerCreated
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string  Email { get; set; }
    public DateOnly DateOfBirth { get; set; }
    
    public required string GitHubUsername { get; init; }
}