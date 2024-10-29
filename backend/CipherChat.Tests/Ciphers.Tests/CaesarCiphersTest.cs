using CipherChat.Ciphers.CaesarCipher;
using FluentAssertions;

namespace CipherChat.Tests.Ciphers.Tests;

public class CaesarCipherTests
{
    [Theory]
    [InlineData("HELLO", "KHOOR", "3", "EN")]
    [InlineData("abc", "def", "3", "EN")]
    [InlineData("XYZ", "ABC", "3", "EN")]
    [InlineData("Hello, World!", "Khoor, Zruog!", "3", "EN")]
    public void Encrypt_ShouldReturnExpectedCiphertext_English(string plainText, string expectedCiphertext, string key, string language)
    {
        var cipher = new CaesarCipherService();

        var result = cipher.Encrypt(plainText, key, language);

        result.Should().Be(expectedCiphertext);
    }

    [Theory]
    [InlineData("KHOOR", "HELLO", "3", "EN")]
    [InlineData("def", "abc", "3", "EN")]
    [InlineData("ABC", "XYZ", "3", "EN")]
    [InlineData("Khoor, Zruog!", "Hello, World!", "3", "EN")]
    public void Decrypt_ShouldReturnExpectedPlaintext_English(string cipherText, string expectedPlaintext, string key, string language)
    {
        var cipher = new CaesarCipherService();

        var result = cipher.Decrypt(cipherText, key, language);

        result.Should().Be(expectedPlaintext);
    }

    [Theory]
    [InlineData("cześć", "fbixg", "5", "PL")]
    [InlineData("ąbćzac", "defąćę", "4", "PL")]
    [InlineData("YZ", "AĄ", "4", "PL")]
    public void Encrypt_ShouldReturnExpectedCiphertext_Polish(string plainText, string expectedCiphertext, string key, string language)
    {
        var cipher = new CaesarCipherService();

        var result = cipher.Encrypt(plainText, key, language);

        result.Should().Be(expectedCiphertext);
    }

    [Theory]
    [InlineData("fbixg", "cześć", "5", "PL")]
    [InlineData("defąćę", "ąbćzac", "4", "PL")]
    [InlineData("AĄ", "YZ", "4", "PL")]
    public void Decrypt_ShouldReturnExpectedPlaintext_Polish(string cipherText, string expectedPlaintext, string key, string language)
    {
        var cipher = new CaesarCipherService();

        var result = cipher.Decrypt(cipherText, key, language);

        result.Should().Be(expectedPlaintext);
    }
}
