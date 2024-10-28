using CipherChat.Ciphers.CaesarCipher;
using CipherChat.Ciphers.PolibiusCipher;
using CipherChat.Ciphers.VigenereCipher;
using CipherChat.Domain.Interfaces;

namespace CipherChat.Ciphers;

public class CipherFactory : ICipherFactory
{
    private readonly IServiceProvider _serviceProvider;

    public CipherFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    } 
    
    public ICipherService GetCipherService(string cipherType)
    {
        return cipherType switch
        {
            "Caesar" => _serviceProvider.GetRequiredService<CaesarCipherService>(),
            "Vigenere" => _serviceProvider.GetRequiredService<VigenereCipherService>(),
            "Playfair" => _serviceProvider.GetRequiredService<PlayfairCipherService>(),
            "Polibius" => _serviceProvider.GetRequiredService<PolibiusCipherService>(),
            _ => throw new ArgumentException("Unsupported cipher type")
        };
    }
}