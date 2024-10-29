namespace CipherChat.Domain.Interfaces;

public interface ICipherFactory
{
    ICipherService GetCipherService(string cipherType);
}
