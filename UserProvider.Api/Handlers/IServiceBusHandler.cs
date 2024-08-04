
namespace UserProvider.Api.Handlers;

public interface IServiceBusHandler
{
    Task ReceiveMessagesAsync<T>(string queueName, Func<T, Task> processMessage);
    Task SendMessageAsync<T>(string queueName, T messageContent);
}