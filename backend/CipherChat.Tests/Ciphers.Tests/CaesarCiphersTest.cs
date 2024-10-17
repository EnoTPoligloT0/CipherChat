using CipherChat.Ciphers.CaesarCipher;
using FluentAssertions;
using Xunit;

namespace CipherChat.Tests.Ciphers.Tests;

public class CaesarCipherTests
{
    private readonly CaesarCipherService _cipher;

    public CaesarCipherTests()
    {
        var settings = new CaesarCipherSettings { Key = 3 };
        _cipher = new CaesarCipherService(settings);
    }

    [Theory]
    [InlineData("HELLO", "KHOOR")]
    [InlineData("abc", "def")]
    [InlineData("XYZ", "ABC")]
    [InlineData("Hello, World!", "Khoor, Zruog!")]
    public void Encrypt_ShouldReturnExpectedCiphertext(string plainText, string expectedCiphertext)
    {
        var result = _cipher.Encrypt(plainText);

        result.Should().Be(expectedCiphertext);
    }

    [Theory]
    [InlineData("KHOOR", "HELLO")]
    [InlineData("def", "abc")]
    [InlineData("ABC", "XYZ")]
    [InlineData("Khoor, Zruog!", "Hello, World!")]
    public void Decrypt_ShouldReturnExpectedPlaintext(string cipherText, string expectedPlaintext)
    {
        var result = _cipher.Decrypt(cipherText);

        result.Should().Be(expectedPlaintext);
    }
}