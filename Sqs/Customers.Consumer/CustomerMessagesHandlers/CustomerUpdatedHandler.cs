using Customers.Consumer.Messages;
using MediatR;

namespace Customers.Consumer.CustomerMessagesHandlers;

public class CustomerUpdatedHandler : IRequestHandler<CustomerUpdated>
{
    private readonly ILogger<CustomerUpdatedHandler> _logger;
    
    public CustomerUpdatedHandler(ILogger<CustomerUpdatedHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public Task<Unit> Handle(CustomerUpdated request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(request.FullName);
        return Unit.Task;
    }
}