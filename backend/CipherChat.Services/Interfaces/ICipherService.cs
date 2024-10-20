namespace CipherChat.Domain.Interfaces;

public interface ICipherService
{
    public string Encrypt(string plainText, int shift, string language);
    public string Decrypt(string cipherText, int shift, string language);
}