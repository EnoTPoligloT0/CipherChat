using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using CipherChat.API.Models;
using CipherChat.Ciphers;
using CipherChat.Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace CipherChat.API.Hubs;

public class ChatHub : Hub<IChatClient>
{
    private readonly IDistributedCache _cache;
    private readonly IServiceProvider _serviceProvider;

    public ChatHub(IDistributedCache cache, IServiceProvider serviceProvider)
    {
        _cache = cache;
        _serviceProvider = serviceProvider;
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



    public async Task SendMessage(string message, string cipherType = null, string language = null, string key = null)
    {
        if (string.IsNullOrEmpty(message))
            throw new ArgumentException("Message cannot be empty", nameof(message));

        var stringConnection = await _cache.GetStringAsync(Context.ConnectionId);
        if (stringConnection == null)
            throw new InvalidOperationException("User connection not found in cache.");

        var connection = JsonSerializer.Deserialize<UserConnection>(stringConnection);
        if (connection == null)
            throw new InvalidOperationException("Deserialization of UserConnection failed.");

        string finalMessage;

        using (var scope = _serviceProvider.CreateScope())
        {
            var cipherFactory = scope.ServiceProvider.GetRequiredService<ICipherFactory>();

            if (!string.IsNullOrEmpty(cipherType) && !string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(language))
            {
                var cipherService = cipherFactory.GetCipherService(cipherType);
                finalMessage = cipherService.Encrypt(message, key, language);
            }
            else
            {
                finalMessage = message;
            }
        }

        await Clients.Group(connection.ChatRoom).ReceiveMessage(connection.UserName, finalMessage);
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