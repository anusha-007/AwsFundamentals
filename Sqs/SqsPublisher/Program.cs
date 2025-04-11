using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using SqsPublisher;

var sqs = new AmazonSQSClient();

var message = new CustomerCreated
{
    Id = Guid.NewGuid(),
    FullName = "Anusha Ramadugu",
    Email = "abc@gmail.com",
    DateOfBirth = new DateOnly(1985, 10, 10),
};
var queueUrlResponse = await sqs.GetQueueUrlAsync("Customers");

var messageRequest = new SendMessageRequest
{
    MessageAttributes = new Dictionary<string, MessageAttributeValue>
    {
        {
            "MessageType", new MessageAttributeValue()
            {
                DataType = "String",
                StringValue = nameof(CustomerCreated)
            }
        }
    },
    QueueUrl = queueUrlResponse.QueueUrl,
    MessageBody = JsonSerializer.Serialize(message),
};

var response = await sqs.SendMessageAsync(messageRequest);

Console.WriteLine($"Sent message to {queueUrlResponse.QueueUrl}");