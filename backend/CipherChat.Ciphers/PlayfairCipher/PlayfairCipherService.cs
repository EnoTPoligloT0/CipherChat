using System.Text;
using CipherChat.Domain.Interfaces;

namespace CipherChat.Ciphers.PlayfairCipher
{
    public class PlayfairCipherService : ICipherService
    {
        public string Encrypt(string plainText, string key, string language)
        {
            var alphabet = AlphabetProvider.GetAlphabet(language);
            var matrix = CreateMatrix(key, alphabet);
            return ProcessText(plainText, matrix, true);
        }

        public string Decrypt(string cipherText, string key, string language)
        {
            var alphabet = AlphabetProvider.GetAlphabet(language);
            var matrix = CreateMatrix(key, alphabet);
            string decryptedText = ProcessText(cipherText, matrix, false);
            return RemoveTrailingPadding(decryptedText);
        }

        public static char[,] CreateMatrix(string keyword, string alphabet)
        {
            int gridSize = alphabet.Length > 26 ? 6 : 5;
            var uniqueChars = new HashSet<char>();

            foreach (char c in keyword)
            {
                if (alphabet.Contains(c))
                {
                    uniqueChars.Add(c);
                }
            }

            foreach (char c in alphabet)
            {
                uniqueChars.Add(c);
            }

            var matrix = new char[gridSize, gridSize];
            var distinctChars = uniqueChars.Take(gridSize * gridSize).ToArray();

            for (int i = 0; i < distinctChars.Length; i++)
            {
                matrix[i / gridSize, i % gridSize] = distinctChars[i];
            }

            return matrix;
        }

        private static string ProcessText(string text, char[,] matrix, bool encrypt)
        {
            int gridSize = matrix.GetLength(0);
            var formattedText = FormatText(text);
            var result = new StringBuilder();

            for (int i = 0; i < formattedText.Length; i += 2)
            {
                char a = formattedText[i];
                char b = formattedText[i + 1];
                (int rowA, int colA) = FindPosition(matrix, a);
                (int rowB, int colB) = FindPosition(matrix, b);

                if (rowA == rowB) 
                {
                    result.Append(matrix[rowA, (colA + (encrypt ? 1 : gridSize - 1)) % gridSize]);
                    result.Append(matrix[rowB, (colB + (encrypt ? 1 : gridSize - 1)) % gridSize]);
                }
                else if (colA == colB) 
                {
                    result.Append(matrix[(rowA + (encrypt ? 1 : gridSize - 1)) % gridSize, colA]);
                    result.Append(matrix[(rowB + (encrypt ? 1 : gridSize - 1)) % gridSize, colB]);
                }
                else
                {
                    result.Append(matrix[rowA, colB]);
                    result.Append(matrix[rowB, colA]);
                }
            }

            return result.ToString();
        }

        private static (int, int) FindPosition(char[,] matrix, char character)
        {
            int gridSize = matrix.GetLength(0);
            for (int row = 0; row < gridSize; row++)
            {
                for (int col = 0; col < gridSize; col++)
                {
                    if (matrix[row, col] == character)
                        return (row, col);
                }
            }

            throw new ArgumentException($"Character '{character}' not found in matrix.");
        }

        private static string FormatText(string text)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < text.Length; i += 2)
            {
                char a = text[i];
                char b = (i + 1 < text.Length && text[i + 1] != a) ? text[i + 1] : 'x';

                sb.Append(a);
                sb.Append(b);
            }

            if (sb.Length % 2 != 0)
                sb.Append('x'); 

            return sb.ToString();
        }

        private static string RemoveTrailingPadding(string text)
        {
            if (text.EndsWith("x") && !text.EndsWith("xx"))
            {
                text = text.Remove(text.Length - 1, 1);
            }
            return text;
        }
    }
}
