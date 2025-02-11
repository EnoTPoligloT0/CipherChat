namespace CipherChat.Ciphers
{
    public static class AlphabetProvider
    {
        public const string PolishAlphabet = "aąbcćdeęfghijklłmnńoóprsśtuvwxyzźż";
        public const string EnglishAlphabet = "abcdefghijklmnopqrstuvwxyz";

        public static string GetAlphabet(string language)
        {
            return language.ToUpper() switch
            {
                "PL" => PolishAlphabet,
                "EN" => EnglishAlphabet,
                _ => throw new ArgumentException("Unsupported language")
            };
        }
    }
}