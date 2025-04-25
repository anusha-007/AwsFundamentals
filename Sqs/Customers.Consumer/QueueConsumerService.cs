using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Options;

namespace Customers.Consumer;

public class QueueConsumerService : BackgroundService
{
    private readonly IAmazonSQS _sqs;
    private readonly ILogger _logger;
    private readonly IOptions<QueueSettings> _queueSettings;


    public QueueConsumerService(IAmazonSQS sqs,
        ILogger logger)
    {
        _sqs = sqs ?? throw new ArgumentNullException(nameof(sqs));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var queueUrl = await GetQueueUrl();
        var receiveMsg = new ReceiveMessageRequest
        {
            MaxNumberOfMessages = 1,
            MessageAttributeNames = new List<string>(){"All"},
            MessageSystemAttributeNames = new List<string>(){"All"},
            QueueUrl = queueUrl,
        };

        while (!stoppingToken.IsCancellationRequested)
        {
            var response = await _sqs.ReceiveMessageAsync(receiveMsg, stoppingToken);

            foreach (var message in response.Messages)
            {
                _logger.LogInformation($"Received message: {message.MessageAttributes["MessageType"]}");
                await _sqs.DeleteMessageAsync(queueUrl, message.ReceiptHandle, stoppingToken);
            }
            await Task.Delay(1000);
        } 
    }

    private async Task<string> GetQueueUrl()
    {
        var queueResponse = await _sqs.GetQueueUrlAsync(_queueSettings.Value.QueueName);
        return queueResponse.QueueUrl;
    }
}