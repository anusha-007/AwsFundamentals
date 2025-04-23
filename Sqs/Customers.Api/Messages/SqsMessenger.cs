using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Options;

namespace Customers.Api.Messages;

public class SqsMessenger : ISqsMessenger
{
    private IAmazonSQS _sqs;
    private readonly IOptions<QueueSettings> _queueSettings;
    private string _queueUrl = null;

    public SqsMessenger(IAmazonSQS sqs, IOptions<QueueSettings> queueSettings)
    {
        _sqs = sqs;
        _queueSettings = queueSettings;
    }


    public async Task<SendMessageResponse> SendMessageAsync<T>(T message)
    {
        var queueUrl = await GetQueueUrl();

        var msg = new SendMessageRequest
        {
           QueueUrl = queueUrl,
           MessageBody = JsonSerializer.Serialize(message),
           MessageAttributes = new Dictionary<string, MessageAttributeValue>()
           {
               {"MessageType", new MessageAttributeValue
               {
                   DataType = "String" ,
                   StringValue = typeof(T).Name
               }}
           }
        };
        
        var res = await _sqs.SendMessageAsync(msg);
        return res;
    }

    private async Task<string> GetQueueUrl()
    {
        if (_queueUrl != null)
        {
            return _queueUrl;
        }
        
        var res = await _sqs.GetQueueUrlAsync(_queueSettings.Value.QueueName);
        _queueUrl = res.QueueUrl;
        return _queueUrl;
    }
    
    
}