using CipherChat.Ciphers.PolibiusCipher;
using FluentAssertions;

namespace CipherChat.Tests.Ciphers.Tests
{
    public class PolibiusCipherTests
    {
        [Theory]
        [InlineData("hello", "2315323235", "EN")]
        [InlineData("ciąg", "14321225", "PL")]
        [InlineData("szyfr", "5465642453", "PL")]
        [InlineData("kot", "344561", "PL")]
        [InlineData("cat", "131145", "EN")]
        [InlineData("h3llo!", "233323235!", "EN")] 
        public void Encrypt_Should_EncryptPlainTextToCipherText(string plainText, string expectedCipherText, string language)
        {
            var cipherService = new PolibiusCipherService(language);

            string encrypted = cipherService.Encrypt(plainText, "0", language);

            encrypted.Should().Be(expectedCipherText, because: $"Plaintext: '{plainText}' Language: {language} ");
        }

        [Theory]
        [InlineData("2315323235", "hello", "EN")]
        [InlineData("14321225", "ciąg", "PL")]
        [InlineData("5465642453", "szyfr", "PL")]
        [InlineData("344561", "kot", "PL")]
        [InlineData("131145", "cat", "EN")]
        public void Decrypt_Should_DecryptCipherTextToPlainText(string cipherText, string expectedPlainText, string language)
        {
            var cipherService = new PolibiusCipherService(language);

            string decrypted = cipherService.Decrypt(cipherText, "0", language);

            decrypted.Should().Be(expectedPlainText, because: $"CipherText: '{cipherText}' should map to Plaintext: '{expectedPlainText}' language {language}.");
        }

        [Theory]
        [InlineData("unknown", "Unsupported language")]
        [InlineData("", "Unsupported language")]
        public void GetAlphabetGrid_Should_ThrowArgumentException_ForUnsupportedLanguage(string language, string expectedMessage)
        {
            Action act = () => new PolibiusCipherService(language);

            act.Should().Throw<ArgumentException>()
                .WithMessage(expectedMessage);
        }
    }
}
