namespace CipherChat.Ciphers.PolibiusCipher;

public static class PolibiusAlphabetProvider
{
    public static string[][] GetAlphabetGrid(string language)
    {
        return language.ToUpper() switch
        {
            "PL" => new string[][]
            {
                new[] { "a", "ą", "b", "c", "ć" },
                new[] { "d", "e", "ę", "f", "g" },
                new[] { "h", "i", "j", "k", "l" },
                new[] { "ł", "m", "n", "ń", "o" },
                new[] { "ó", "p", "r", "s", "ś" },
                new[] { "t", "u", "w", "y", "z" },
                new[] { "ź", "ż" }  
            },
            "EN" => new string[][]
            {
                new[] { "a", "b", "c", "d", "e" },
                new[] { "f", "g", "h", "i", "j" },
                new[] { "k", "l", "m", "n", "o" },
                new[] { "p", "q", "r", "s", "t" },
                new[] { "u", "v", "w", "x", "y" },
                new[] { "z" }
            },
            _ => throw new ArgumentException("Unsupported language")
        };
    }
}