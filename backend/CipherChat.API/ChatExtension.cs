using CipherChat.Ciphers;
using CipherChat.Ciphers.CaesarCipher;
using CipherChat.Ciphers.PlayfairCipher;
using CipherChat.Ciphers.PolibiusCipher;
using CipherChat.Ciphers.VigenereCipher;
using CipherChat.Domain.Interfaces;

namespace CipherChat.API;

public class ChatExtension
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<CaesarCipherService>();
        services.AddScoped<VigenereCipherService>();
        services.AddScoped<PlayfairCipherService>();
        services.AddScoped<PolibiusCipherService>();
        
        services.AddSingleton<ICipherFactory, CipherFactory>();
    
        services.AddSignalR();
    }

}