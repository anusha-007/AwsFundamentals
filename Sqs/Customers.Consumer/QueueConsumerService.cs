using System.Text.Json;
using System.Text.Json.Serialization;
using Amazon.SQS;
using Amazon.SQS.Model;
using Customers.Consumer.Messages;
using MediatR;
using Microsoft.Extensions.Options;

namespace Customers.Consumer;

public class QueueConsumerService : BackgroundService
{
    private IAmazonSQS _sqs;
    private readonly IOptions<QueueSettings> _queueSettings;
    private readonly IMediator _mediator;
    private readonly ILogger<QueueConsumerService> _logger;

    public QueueConsumerService(IAmazonSQS sqs, IOptions<QueueSettings> queueSettings,
        IMediator mediator, ILogger<QueueConsumerService> logger)
    {
        _sqs = sqs ?? throw new ArgumentNullException(nameof(sqs));
        _queueSettings = queueSettings;
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
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

            if(response.Messages?.Count == 0 || response.Messages == null) break;

            foreach (var message in response.Messages)
            {
                var messageType = message.MessageAttributes["MessageType"].StringValue;
                var type = Type.GetType($"Customers.Consumer.Messages.{messageType}");

                if (type == null)
                {
                    _logger.LogWarning("Unkown message type {MessageType}", messageType);
                }
                var typedMessage = (ISqsMessage)JsonSerializer.Deserialize(message.Body, type);
                
                try
                {
                    await _mediator.Send(typedMessage, stoppingToken);
                    
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Message failed during processing {MessageType}", messageType);
                    continue;
                }
                
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