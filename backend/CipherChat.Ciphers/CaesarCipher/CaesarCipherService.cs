using System.Text;
using CipherChat.Domain.Interfaces;

namespace CipherChat.Ciphers.CaesarCipher
{
    public class CaesarCipherService : ICipherService
    {
        public string Encrypt(string plainText, string key, string language)
        {
            var shift = Convert.ToInt32(key); 
            var alphabet = AlphabetProvider.GetAlphabet(language);
            return ProcessText(plainText, shift, alphabet, true);
        }

        public string Decrypt(string cipherText, string key, string language)
        {
            var shift = Convert.ToInt32(key); 
            var alphabet = AlphabetProvider.GetAlphabet(language);
            return ProcessText(cipherText, shift, alphabet, false);
        }

        private string ProcessText(string text, int shift, string alphabet, bool isEncryption)
        {
            var result = new StringBuilder();
            foreach (var character in text)
            {
                result.Append(ProcessCharacter(character, shift, alphabet, isEncryption));
            }
            return result.ToString();
        }

        private char ProcessCharacter(char character, int shift, string alphabet, bool isEncryption)
        {
            var targetChar = char.ToLower(character);
            if (alphabet.Contains(targetChar))
            {
                var alphabetIndex = alphabet.IndexOf(targetChar);
                var shiftAmount = isEncryption ? shift : -shift;
                var newCharIndex = (alphabetIndex + shiftAmount + alphabet.Length) % alphabet.Length;
                var resultChar = alphabet[newCharIndex];

                return char.IsUpper(character) ? char.ToUpper(resultChar) : resultChar;
            }
            return character;
        }
    }
}