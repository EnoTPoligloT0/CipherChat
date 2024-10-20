using System.Text;
using CipherChat.Domain.Interfaces;

namespace CipherChat.Ciphers.CaesarCipher
{
    public class CaesarCipherService : ICipherService
    {
        public string Encrypt(string plainText, int shift, string language)
        {
            string alphabet = GetAlphabet(language);
            StringBuilder encrypted = new StringBuilder();
            foreach (char character in plainText)
            {
                char targetChar = char.ToLower(character);
                if (alphabet.Contains(targetChar))
                {
                    int alphabetIndex = alphabet.IndexOf(targetChar);
                    // Shift character forward by shift positions
                    int newCharIndex = (alphabetIndex + shift) % alphabet.Length;
                    char encryptedChar = alphabet[newCharIndex];

                    // Preserve the case of the original character
                    encrypted.Append(char.IsUpper(character) ? char.ToUpper(encryptedChar) : encryptedChar);
                }
                else
                {
                    encrypted.Append(character); // Append unchanged characters
                }
            }
            return encrypted.ToString();
        }

        public string Decrypt(string cipherText, int shift, string language)
        {
            string alphabet = GetAlphabet(language);
            StringBuilder decrypted = new StringBuilder();
            foreach (char character in cipherText)
            {
                char targetChar = char.ToLower(character);
                if (alphabet.Contains(targetChar))
                {
                    int alphabetIndex = alphabet.IndexOf(targetChar);
                    // Adjust key for negative shift in decryption
                    int newCharIndex = (alphabetIndex - shift + alphabet.Length) % alphabet.Length;
                    char decryptedChar = alphabet[newCharIndex];

                    // Preserve the case of the original character
                    decrypted.Append(char.IsUpper(character) ? char.ToUpper(decryptedChar) : decryptedChar);
                }
                else
                {
                    decrypted.Append(character); // Append unchanged characters
                }
            }
            return decrypted.ToString();
        }

        private string GetAlphabet(string language)
        {
            return language.ToUpper() switch
            {
                "POLISH" => CipherAlphabets.PolishAlphabet,
                "ENGLISH" => CipherAlphabets.EnglishAlphabet,
                _ => throw new ArgumentException("Unsupported language")
            };
        }
    }
}
