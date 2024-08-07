using Azure.Messaging.ServiceBus;
using Moq;
using System.Text.Json;
using UserProvider.Api.Handlers;
using UserProvider.Data.Models;

namespace UserProvider.Tests.UnitTests;

public class ServiceBusHandlerTests
{
    private readonly Mock<ServiceBusClient> _mockServiceBusClient;
    private readonly Mock<ServiceBusSender> _mockSender;
    private readonly ServiceBusHandler _serviceBusHandler;

    public ServiceBusHandlerTests()
    {
        _mockServiceBusClient = new Mock<ServiceBusClient>();
        _mockSender = new Mock<ServiceBusSender>();

        _mockServiceBusClient.Setup(client => client.CreateSender(It.IsAny<string>()))
            .Returns(_mockSender.Object);

        _serviceBusHandler = new ServiceBusHandler(_mockServiceBusClient.Object);
    }

    [Fact]
    public async Task SendMessageAsync_ShouldSendMessage()
    {
        // Arrange
        var queueName = "test-queue";
        var messageContent = new UserAccountModel
        {
            UserId = "123",
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com"
        };

        var message = JsonSerializer.Serialize(messageContent);

        // Act
        await _serviceBusHandler.SendMessageAsync(queueName, messageContent);

        // Assert
        _mockSender.Verify(sender => sender.SendMessageAsync(It.Is<ServiceBusMessage>(msg =>
            msg.Body.ToString() == message &&
            msg.Subject == null && 
            msg.ContentType == null), CancellationToken.None));
    }
}
