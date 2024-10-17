using System.Text;
using CipherChat.Domain.Interfaces;

namespace CipherChat.Ciphers.CaesarCipher;

public class CaesarCipherService : ICipherService
{
    private readonly int _key;
    
    public CaesarCipherService(CaesarCipherSettings settings)
    {
        _key = settings.Key;
    }
    
    public string Encrypt(string plainText)
    {
        // Implement the encryption logic here
        StringBuilder encrypted = new StringBuilder();
        foreach (char character in plainText)
        {
            if (char.IsLetter(character))
            {
                char offset = char.IsUpper(character) ? 'A' : 'a';
                char encryptedChar = (char)((((character + _key) - offset) % 26) + offset);
                encrypted.Append(encryptedChar);
            }
            else
            {
                encrypted.Append(character); // Non-letter characters are unchanged
            }
        }
        return encrypted.ToString();
    }

    public string Decrypt(string cipherText)
    {
        // Implement the decryption logic here
        StringBuilder decrypted = new StringBuilder();
        foreach (char character in cipherText)
        {
            if (char.IsLetter(character))
            {
                char offset = char.IsUpper(character) ? 'A' : 'a';
                char decryptedChar = (char)((((character - _key) - offset + 26) % 26) + offset);
                decrypted.Append(decryptedChar);
            }
            else
            {
                decrypted.Append(character); // Non-letter characters are unchanged
            }
        }
        return decrypted.ToString();
    }
}