namespace CipherChat.Ciphers.PolibiusCipher;

public static class PolibiusAlphabetProvider
{
    public static string[][] GetAlphabetGrid(string language)
    {
        return language.ToUpper() switch
        {
            "PL" => new[]
            {
                ["a", "ą", "b", "c", "ć"],
                ["d", "e", "ę", "f", "g"],
                ["h", "i", "j", "k", "l"],
                ["ł", "m", "n", "ń", "o"],
                ["ó", "p", "r", "s", "ś"],
                ["t", "u", "w", "y", "z"],
                new[] { "ź", "ż" }  
            },
            "EN" => new[]
            {
                ["a", "b", "c", "d", "e"],
                ["f", "g", "h", "i", "j"],
                ["k", "l", "m", "n", "o"],
                ["p", "q", "r", "s", "t"],
                ["u", "v", "w", "x", "y"],
                new[] { "z" }
            },
            _ => throw new ArgumentException("Unsupported language")
        };
    }
}