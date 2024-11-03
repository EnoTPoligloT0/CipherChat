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
        private readonly Mock<ICipherFactory> _cipherFactoryMock;

        public ChatHubTests(Mock<ICipherFactory> cipherFactoryMock)
        {
            _cacheMock = new Mock<IDistributedCache>();
            _clientsMock = new Mock<IHubCallerClients<IChatClient>>();
            _clientMock = new Mock<IChatClient>();
            _contextMock = new Mock<HubCallerContext>();
            _groupManagerMock = new Mock<IGroupManager>();
            _cipherFactoryMock = new Mock<ICipherFactory>();

            _contextMock.Setup(c => c.ConnectionId).Returns("connection-id");
            _chatHub = new API.Hubs.ChatHub(_cacheMock.Object, _cipherFactoryMock.Object)
            {
                Clients = _clientsMock.Object,
                Context = _contextMock.Object,
                Groups = _groupManagerMock.Object
            };

            _clientsMock.Setup(c => c.Group(It.IsAny<string>())).Returns(_clientMock.Object);
        }

        [Theory]
        [InlineData("User1", "Room1")]
        public async Task JoinChat_ShouldAddUserToGroup(string userName, string roomName)
        {
            var connection = new UserConnection(userName, roomName);
            await _chatHub.JoinChat(connection);
            _groupManagerMock.Verify(g => g.AddToGroupAsync("connection-id", roomName, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData("User1", "Room1")]
        public async Task JoinChat_ShouldCacheUserConnection(string userName, string roomName)
        {
            var connection = new UserConnection(userName, roomName);
            var expectedCacheValue = JsonSerializer.Serialize(connection);
            var expectedBytes = System.Text.Encoding.UTF8.GetBytes(expectedCacheValue);

            await _chatHub.JoinChat(connection);

            _cacheMock.Verify(c => c.SetAsync("connection-id", It.Is<byte[]>(b => b.SequenceEqual(expectedBytes)), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData("User1", "Room1")]
        public async Task JoinChat_ShouldSendWelcomeMessage(string userName, string roomName)
        {
            var connection = new UserConnection(userName, roomName);
            await _chatHub.JoinChat(connection);
            _clientMock.Verify(c => c.ReceiveMessage("Admin", $"{userName} has joined."), Times.Once);
        }

        [Theory]
        [InlineData(null)]
        public async Task JoinChat_ShouldThrowException_WhenConnectionIsNull(UserConnection connection)
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _chatHub.JoinChat(connection));
        }

        [Theory]
        [InlineData("User1", "")]
        public async Task JoinChat_ShouldThrowException_WhenChatRoomIsNullOrEmpty(string userName, string roomName)
        {
            var connection = new UserConnection(userName, roomName);
            await Assert.ThrowsAsync<ArgumentException>(() => _chatHub.JoinChat(connection));
        }

        [Theory]
        [InlineData("Hello, World!")]
        public async Task SendMessage_ShouldSendMessageToGroup(string message)
        {
            var connection = new UserConnection("User1", "Room1");
            var expectedCacheValue = JsonSerializer.Serialize(connection);
            var expectedBytes = System.Text.Encoding.UTF8.GetBytes(expectedCacheValue);

            _cacheMock.Setup(c => c.GetAsync("connection-id", It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedBytes);

            await _chatHub.SendMessage(message);

            _clientMock.Verify(c => c.ReceiveMessage("User1", message), Times.Once);
        }

        [Theory]
        [InlineData("")]
        public async Task SendMessage_ShouldThrowException_WhenMessageIsNullOrEmpty(string message)
        {
            var connection = new UserConnection("User1", "Room1");
            var expectedCacheValue = JsonSerializer.Serialize(connection);
            await _cacheMock.Object.SetStringAsync("connection-id", expectedCacheValue);

            await Assert.ThrowsAsync<ArgumentException>(() => _chatHub.SendMessage(message));
        }

        // [Fact]
        // public async Task SendMessage_ShouldThrowException_WhenUserNotFoundInCache()
        // {
        //     await Assert.ThrowsAsync<InvalidOperationException>(() => _chatHub.SendMessage("Test message"));
        // }

        [Theory]
        [InlineData("User1", "Room1")]
        public async Task OnDisconnectedAsync_ShouldRemoveUserFromGroup(string userName, string roomName)
        {
            var connection = new UserConnection(userName, roomName);
            var expectedCacheValue = JsonSerializer.Serialize(connection);
            var expectedBytes = System.Text.Encoding.UTF8.GetBytes(expectedCacheValue);

            _cacheMock.Setup(c => c.GetAsync("connection-id", It.IsAny<CancellationToken>())).ReturnsAsync(expectedBytes);

            await _chatHub.OnDisconnectedAsync(null);

            _groupManagerMock.Verify(g => g.RemoveFromGroupAsync("connection-id", roomName, It.IsAny<CancellationToken>()), Times.Once);
            _clientMock.Verify(c => c.ReceiveMessage("Admin", $"{userName} disconnected"), Times.Once);
            _cacheMock.Verify(c => c.RemoveAsync("connection-id", It.IsAny<CancellationToken>()), Times.Once);
        }

        // [Fact]
        // public async Task OnDisconnectedAsync_ShouldHandleNonexistentCacheEntry()
        // {
        //     _cacheMock.Setup(c => c.GetAsync("connection-id", It.IsAny<CancellationToken>())).ReturnsAsync((byte[])null);
        //
        //     await _chatHub.OnDisconnectedAsync(null);
        //
        //     _groupManagerMock.Verify(g => g.RemoveFromGroupAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        //     _clientMock.Verify(c => c.ReceiveMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        // }
    }
}
