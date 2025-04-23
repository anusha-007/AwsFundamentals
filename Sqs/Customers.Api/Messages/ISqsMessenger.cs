using Amazon.SQS.Model;

namespace Customers.Api.Messages;

public interface ISqsMessenger
{
     Task<SendMessageResponse> SendMessageAsync<T>(T message);
}