using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using CipherChat.API.Models;
using CipherChat.Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Xunit;

namespace CipherChat.Tests.ChatHub.Tests
{
    public class ChatHubTests
    {
        private readonly Mock<IDistributedCache> _cacheMock;
        private readonly Mock<IHubCallerClients<IChatClient>> _clientsMock;
        private readonly Mock<IChatClient> _clientMock;
        private readonly API.Hubs.ChatHub _chatHub;
        private readonly Mock<HubCallerContext> _contextMock;
        private readonly Mock<IGroupManager> _groupManagerMock;

        public ChatHubTests()
        {
            _cacheMock = new Mock<IDistributedCache>();
            _clientsMock = new Mock<IHubCallerClients<IChatClient>>();
            _clientMock = new Mock<IChatClient>();
            _contextMock = new Mock<HubCallerContext>();
            _groupManagerMock = new Mock<IGroupManager>();

            _contextMock.Setup(c => c.ConnectionId).Returns("connection-id");

            _chatHub = new API.Hubs.ChatHub(_cacheMock.Object)
            {
                Clients = _clientsMock.Object,
                Context = _contextMock.Object
            };

            _chatHub.Groups = _groupManagerMock.Object;

            _clientsMock.Setup(c => c.Group(It.IsAny<string>())).Returns(_clientMock.Object);
        }

        [Fact]
        public async Task JoinChat_ShouldAddUserToGroup()
        {
            var connection = new UserConnection("User1", "Room1");
            await _chatHub.JoinChat(connection);
            _groupManagerMock.Verify(g => g.AddToGroupAsync("connection-id", "Room1", It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task JoinChat_ShouldCacheUserConnection()
        {
            var connection = new UserConnection("User1", "Room1");
            var expectedCacheValue = JsonSerializer.Serialize(connection);
            var expectedBytes = System.Text.Encoding.UTF8.GetBytes(expectedCacheValue);

            await _chatHub.JoinChat(connection);

            _cacheMock.Verify(
                c => c.SetAsync(
                    "connection-id",
                    It.Is<byte[]>(b => b.SequenceEqual(expectedBytes)),
                    It.IsAny<DistributedCacheEntryOptions>(),
                    It.IsAny<CancellationToken>()
                ),
                Times.Once
            );
        }

        [Fact]
        public async Task JoinChat_ShouldSendWelcomeMessage()
        {
            var connection = new UserConnection("User1", "Room1");
            await _chatHub.JoinChat(connection);
            _clientMock.Verify(c => c.ReceiveMessage("Admin", "User1 has joined."), Times.Once);
        }

        [Fact]
        public async Task JoinChat_ShouldThrowException_WhenConnectionIsNull()
        {
            UserConnection connection = null;
            await Assert.ThrowsAsync<ArgumentNullException>(() => _chatHub.JoinChat(connection));
        }

    }
}
