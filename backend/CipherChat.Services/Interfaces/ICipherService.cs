namespace CipherChat.Domain.Interfaces;

public interface ICipherService
{
    public string Encrypt(string plainText);
    public string Decrypt(string cipherText);
}