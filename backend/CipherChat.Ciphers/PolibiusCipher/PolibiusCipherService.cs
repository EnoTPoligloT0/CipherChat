using System.Text;
using CipherChat.Domain.Interfaces;

namespace CipherChat.Ciphers.PolibiusCipher
{
    public class PolibiusCipherService : ICipherService
    {
        private readonly string[][] _alphabetGrid;

        public PolibiusCipherService(string language)
        {
            _alphabetGrid = PolibiusAlphabetProvider.GetAlphabetGrid(language);
        }

        public string Encrypt(string plainText, string key, string language)
        {
            return ProcessText(plainText, true);
        }

        public string Decrypt(string cipherText, string key, string language)
        {
            return ProcessText(cipherText, false);
        }

        private string ProcessText(string text, bool isEncryption)
        {
            StringBuilder result = new StringBuilder();

            if (isEncryption)
            {
                foreach (char character in text.ToLower())
                {
                    string coordinates = GetCharacterCoordinates(character);
                    result.Append(coordinates ?? character.ToString()); // Append the original character if not found
                }
            }
            else
            {
                for (int i = 0; i < text.Length; i += 2)
                {
                    if (i + 1 < text.Length)
                    {
                        string rowChar = text[i].ToString();
                        string colChar = text[i + 1].ToString();
                        char decodedChar = GetCharacterFromCoordinates(rowChar, colChar);
                        result.Append(decodedChar != default ? decodedChar : $"{rowChar}{colChar}");
                    }
                    else
                    {
                        // Handle odd-length cipherText gracefully by appending the last character
                        result.Append(text[i]);
                    }
                }
            }
            return result.ToString();
        }

        private string GetCharacterCoordinates(char character)
        {
            for (int row = 0; row < _alphabetGrid.Length; row++)
            {
                for (int col = 0; col < _alphabetGrid[row].Length; col++)
                {
                    if (_alphabetGrid[row][col].Equals(character.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        return $"{row + 1}{col + 1}";  // 1-based index
                    }
                }
            }
            return null; // Return null if character not found
        }

        private char GetCharacterFromCoordinates(string rowChar, string colChar)
        {
            if (int.TryParse(rowChar, out int row) && int.TryParse(colChar, out int col))
            {
                // Convert to 0-based indices
                row--;
                col--;

                // Check if indices are valid
                if (row >= 0 && row < _alphabetGrid.Length && col >= 0 && col < _alphabetGrid[row].Length)
                {
                    return _alphabetGrid[row][col][0]; // Return first character of the string
                }
            }
            return default; // Return default char if not found
        }
    }
}
