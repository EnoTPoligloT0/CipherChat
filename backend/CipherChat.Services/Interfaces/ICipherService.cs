namespace CipherChat.Domain.Interfaces;

public interface ICipherService
{
    public string Encrypt(string plainText, string key, string language);
    public string Decrypt(string cipherText, string key, string language);
}