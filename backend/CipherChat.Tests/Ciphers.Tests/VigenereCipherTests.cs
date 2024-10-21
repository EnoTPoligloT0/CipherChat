using CipherChat.Ciphers.VigenereCipher;
using Xunit;
using FluentAssertions;

namespace CipherChat.Ciphers.Tests
{
    public class VigenereCipherServiceTests
    {
        [Theory]
        [InlineData("hello", "secret", "zincs", "EN")]
        [InlineData("world", "abc", "wptle", "EN")]
        [InlineData("Witaj", "klucz", "Ętncg", "PL")] // Mixed case for Polish
        public void Encrypt_ShouldReturnEncryptedText(string plainText, string key, string expectedCipherText, string language)
        {
            var cipherService = new VigenereCipherService(language);

            var result = cipherService.Encrypt(plainText, key);

            result.Should().Be(expectedCipherText);
        }

        [Theory]
        [InlineData("zincs", "secret", "hello", "EN")]
        [InlineData("wptle", "ABC", "world", "EN")]
        [InlineData("Ętncg", "klucz", "Witaj", "PL")] // Mixed case for Polish
        public void Decrypt_ShouldReturnDecryptedText(string cipherText, string key, string expectedPlainText, string language)
        {
            var cipherService = new VigenereCipherService(language);

            var result = cipherService.Decrypt(cipherText, key);

            result.Should().Be(expectedPlainText);
        }
    }
}