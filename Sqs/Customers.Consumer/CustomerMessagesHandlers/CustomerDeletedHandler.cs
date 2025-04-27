using Customers.Consumer.Messages;
using MediatR;

namespace Customers.Consumer.CustomerMessagesHandlers;

public class CustomerDeletedHandler : IRequestHandler<CustomerDeleted>
{
    private readonly ILogger<CustomerDeletedHandler> _logger;
    
    public CustomerDeletedHandler(ILogger<CustomerDeletedHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public Task<Unit> Handle(CustomerDeleted request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(request.Id.ToString());
        
        return Unit.Task;
    }
}