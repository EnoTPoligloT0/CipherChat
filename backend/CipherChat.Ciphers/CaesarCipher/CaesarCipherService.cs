using System.Text;
using CipherChat.Domain.Interfaces;

namespace CipherChat.Ciphers.CaesarCipher
{
    public class CaesarCipherService : ICipherService
    {
        public string Encrypt(string plainText, string key, string language)
        {
            int shift = Convert.ToInt32(key); 
            string alphabet = AlphabetProvider.GetAlphabet(language);
            return ProcessText(plainText, shift, alphabet, true);
        }

        public string Decrypt(string cipherText, string key, string language)
        {
            int shift = Convert.ToInt32(key); 
            string alphabet = AlphabetProvider.GetAlphabet(language);
            return ProcessText(cipherText, shift, alphabet, false);
        }

        private string ProcessText(string text, int shift, string alphabet, bool isEncryption)
        {
            StringBuilder result = new StringBuilder();
            foreach (char character in text)
            {
                result.Append(ProcessCharacter(character, shift, alphabet, isEncryption));
            }
            return result.ToString();
        }

        private char ProcessCharacter(char character, int shift, string alphabet, bool isEncryption)
        {
            char targetChar = char.ToLower(character);
            if (alphabet.Contains(targetChar))
            {
                int alphabetIndex = alphabet.IndexOf(targetChar);
                int shiftAmount = isEncryption ? shift : -shift;
                int newCharIndex = (alphabetIndex + shiftAmount + alphabet.Length) % alphabet.Length;
                char resultChar = alphabet[newCharIndex];

                return char.IsUpper(character) ? char.ToUpper(resultChar) : resultChar;
            }
            return character;
        }
    }
}