using Azure.Messaging.ServiceBus;
using System.Diagnostics;
using System.Text.Json;

namespace UserProvider.Api.Handlers;

public class ServiceBusHandler(ServiceBusClient serviceBusClient) : IServiceBusHandler
{
    private readonly ServiceBusClient _serviceBusClient = serviceBusClient;

    public async Task SendMessageAsync<T>(string queueName, T messageContent)
    {
        var sender = _serviceBusClient.CreateSender(queueName);

        var message = new ServiceBusMessage(JsonSerializer.Serialize(messageContent));
        try
        {
            await sender.SendMessageAsync(message);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    public async Task ReceiveMessagesAsync<T>(string queueName, Func<T, Task> processMessage)
    {
        var processor = _serviceBusClient.CreateProcessor(queueName, new ServiceBusProcessorOptions());

        processor.ProcessMessageAsync += async (args) =>
        {
            try
            {
                var messageBody = args.Message.Body.ToString();
                var messageContent = JsonSerializer.Deserialize<T>(messageBody);

                if (messageContent != null)
                {
                    await processMessage(messageContent);
                }

                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        };

        processor.ProcessErrorAsync += async (args) =>
        {
            Debug.WriteLine(args.Exception.Message);
            await Task.CompletedTask;
        };

        await processor.StartProcessingAsync();
    }
}
