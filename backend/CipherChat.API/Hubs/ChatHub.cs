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
        if (connection == null)
        {
            throw new ArgumentNullException(nameof(connection), "UserConnection cannot be null.");
        }

        if (string.IsNullOrEmpty(connection.ChatRoom))
        {
            throw new ArgumentException("ChatRoom cannot be null or empty.", nameof(connection.ChatRoom));
        }

        if (Context == null)
        {
            throw new InvalidOperationException("Context is not set.");
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
        try
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentException("Message cannot be null or empty.", nameof(message));
            }
            var stringConnection = await _cache.GetStringAsync(Context.ConnectionId);
            if (stringConnection == null)
            {
                throw new InvalidOperationException("User connection not found in cache.");
            }

            var connection = JsonSerializer.Deserialize<UserConnection>(stringConnection);
            if (connection == null)
            {
                throw new InvalidOperationException("Deserialization of UserConnection failed.");
            }

            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentException("Message cannot be null or empty.", nameof(message));
            }

            await Clients
                .Group(connection.ChatRoom)
                .ReceiveMessage(connection.UserName, message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in SendMessage: {ex.Message}");
            throw;
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var stringConnection = await _cache.GetAsync(Context.ConnectionId);

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