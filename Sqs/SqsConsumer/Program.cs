using Amazon.SQS;
using Amazon.SQS.Model;

var cts = new CancellationTokenSource();
var sqsClient =  new AmazonSQSClient();

var queueUrlResponse =await sqsClient.GetQueueUrlAsync("Customers");

var receiveMessageRequest = new ReceiveMessageRequest
{
    QueueUrl = queueUrlResponse.QueueUrl,
    AttributeNames = new List<string>{"All"},
    MessageAttributeNames =  new List<string>{"All"},
};

while (!cts.IsCancellationRequested)
{
    var res = await sqsClient.ReceiveMessageAsync(receiveMessageRequest, cts.Token);

    Console.WriteLine("Message received" + res.Messages?.Count);
    if(res.Messages?.Count == 0 || res.Messages == null) break;
    foreach (var message in res.Messages)
    {
        Console.WriteLine(message);
        Console.WriteLine($"message id: {message.MessageId}");
        Console.WriteLine($"message body: {message.Body}");
        await sqsClient.DeleteMessageAsync(queueUrlResponse.QueueUrl, message.ReceiptHandle, cts.Token);
    }

    await Task.Delay(3000);
    
}
