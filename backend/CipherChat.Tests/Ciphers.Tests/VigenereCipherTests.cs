using CipherChat.Ciphers.VigenereCipher;
using FluentAssertions;

namespace CipherChat.Tests.Ciphers.Tests
{
    public class VigenereCipherServiceTests
    {
        [Theory]
        [InlineData("hello", "secret", "zincs", "EN")]
        [InlineData("world", "abc", "wptle", "EN")]
        [InlineData("Witaj", "klucz", "Ętncg", "PL")] 
        public void Encrypt_ShouldReturnEncryptedText(string plainText, string key, string expectedCipherText, string language)
        {
            var cipherService = new VigenereCipherService();

            var result = cipherService.Encrypt(plainText, key, language);

            result.Should().Be(expectedCipherText);
        }

        [Theory]
        [InlineData("zincs", "secret", "hello", "EN")]
        [InlineData("wptle", "ABC", "world", "EN")]
        [InlineData("Ętncg", "klucz", "Witaj", "PL")] 
        public void Decrypt_ShouldReturnDecryptedText(string cipherText, string key, string expectedPlainText, string language)
        {
            var cipherService = new VigenereCipherService();

            var result = cipherService.Decrypt(cipherText, key, language);

            result.Should().Be(expectedPlainText);
        }
    }
}