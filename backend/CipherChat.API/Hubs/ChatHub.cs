using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using CipherChat.API.Models;
using CipherChat.Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace CipherChat.API.Hubs;

public class ChatHub : Hub<IChatClient>
{
    private readonly IDistributedCache _cache;

    public ChatHub(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task JoinChat(UserConnection connection)
    {
        if (connection == null || string.IsNullOrEmpty(connection.ChatRoom))
        {
            throw new ArgumentNullException("Connection details are missing.");
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatRoom);

        var stringConnection = JsonSerializer.Serialize(connection);
        await _cache.SetStringAsync(Context.ConnectionId, stringConnection);

        await Clients
            .Group(connection.ChatRoom)
            .ReceiveMessage("Admin", $"{connection.UserName} has joined.");
    }

    public async Task SendMessage(string message)
    {
        if (string.IsNullOrEmpty(message)) throw new ArgumentException("Message cannot be empty");

        var stringConnection = await _cache.GetStringAsync(Context.ConnectionId);
        if (stringConnection == null) throw new InvalidOperationException("User connection not found in cache.");

        var connection = JsonSerializer.Deserialize<UserConnection>(stringConnection);
        if (connection == null) throw new InvalidOperationException("Deserialization of UserConnection failed.");

        // Send message only to clients in the same chat room
        await Clients.Group(connection.ChatRoom).ReceiveMessage(connection.UserName, message);
    }


    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var stringConnection = await _cache.GetStringAsync(Context.ConnectionId);

        var connection = JsonSerializer.Deserialize<UserConnection>(stringConnection);

        if (connection is not null)
        {
            await _cache.RemoveAsync(Context.ConnectionId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, connection.ChatRoom);

            await Clients
                .Group(connection.ChatRoom)
                .ReceiveMessage("Admin", $"{connection.UserName} disconnected");
        }

        await base.OnDisconnectedAsync(exception);
    }
}