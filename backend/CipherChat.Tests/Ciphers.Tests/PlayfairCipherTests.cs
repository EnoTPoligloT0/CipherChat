using CipherChat.Ciphers;
using FluentAssertions;

namespace CipherChat.Tests.Ciphers.Tests;

 public class PlayfairCipherTests
    {
        [Theory]
        [InlineData("hello", "keyword", "EN", "gyqsco")]
        [InlineData("world", "keyword", "EN", "okfsct")]
        [InlineData("hi", "keyword", "EN", "ij")]
        [InlineData("dzieńdobry", "słowo", "PL", "cżjduisćys")]
        [InlineData("cień", "słowo", "PL", "dgdó")]
        public void Encrypt_ShouldEncryptPlaintextCorrectly(string plaintext, string keyword, string language, string expected)
        {
            var result = PlayfairCipher.Encrypt(plaintext, keyword, language);
            result.Should().Be(expected);
        }

        [Theory]
         [InlineData("okfsct", "keyword", "EN", "world")]
        [InlineData("ij", "keyword", "EN", "hi")]
        [InlineData("cżjduisćys", "słowo", "PL", "dzieńdobry")]
        [InlineData("dgdó", "słowo", "PL", "cień")]
        public void Decrypt_ShouldDecryptCiphertextCorrectly(string ciphertext, string keyword, string language, string expected)
        {
            var result = PlayfairCipher.Decrypt(ciphertext, keyword, language);
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("keyword", "EN")]
        [InlineData("world", "EN")]
        public void CreateMatrix_ShouldContainDistinctCharacters(string keyword, string language)
        {
            var alphabet = AlphabetProvider.GetAlphabet(language);
            var matrix = PlayfairCipher.CreateMatrix(keyword, alphabet);
            
            var elements = matrix.Cast<char>().ToArray();
            elements.Should().OnlyHaveUniqueItems();
        }

        [Theory]
        [InlineData("PL", "aąbcćdeęfghijklłmnńoóprsśtuvwxyzźż")]
        [InlineData("EN", "abcdefghijklmnopqrstuvwxyz")]
        public void GetAlphabet_ShouldReturnCorrectAlphabetForLanguage(string language, string expected)
        {
            var result = AlphabetProvider.GetAlphabet(language);
            result.Should().Be(expected);
        }

        [Fact]
        public void Encrypt_ShouldThrowException_ForUnsupportedLanguage()
        {
            Action act = () => PlayfairCipher.Encrypt("hello", "keyword", "DE");
            act.Should().Throw<ArgumentException>().WithMessage("Unsupported language");
        }
    }