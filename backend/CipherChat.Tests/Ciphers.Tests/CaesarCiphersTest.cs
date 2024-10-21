using CipherChat.Ciphers.CaesarCipher;
using FluentAssertions;
using Xunit;

namespace CipherChat.Tests.Ciphers.Tests;

public class CaesarCipherTests
{
    [Theory]
    [InlineData("HELLO", "KHOOR", 3, "ENGLISH")]
    [InlineData("abc", "def", 3, "ENGLISH")]
    [InlineData("XYZ", "ABC", 3, "ENGLISH")]
    [InlineData("Hello, World!", "Khoor, Zruog!", 3, "ENGLISH")]
    public void Encrypt_ShouldReturnExpectedCiphertext_English(string plainText, string expectedCiphertext,
        int shift, string language)
    {
        var cipher = new CaesarCipherService();

        var result = cipher.Encrypt(plainText, shift, language);

        result.Should().Be(expectedCiphertext); // Compare exactly as-is.
    }

    [Theory]
    [InlineData("KHOOR", "HELLO", 3, "ENGLISH")]
    [InlineData("def", "abc", 3, "ENGLISH")]
    [InlineData("ABC", "XYZ", 3, "ENGLISH")]
    [InlineData("Khoor, Zruog!", "Hello, World!", 3, "ENGLISH")]
    public void Decrypt_ShouldReturnExpectedPlaintext_English(string cipherText, string expectedPlaintext,
        int shift, string language)
    {
        var cipher = new CaesarCipherService();

        var result = cipher.Decrypt(cipherText, shift, language);

        result.Should().Be(expectedPlaintext); 
    }

    [Theory]
    [InlineData("cześć", "fbixg", 5, "POLISH")] 
    [InlineData("ąbćzac", "defąćę", 4, "POLISH")]
    [InlineData("YZ", "AĄ", 4, "POLISH")] 
    public void Encrypt_ShouldReturnExpectedCiphertext_Polish(string plainText, string expectedCiphertext,
        int shift, string language)
    {
        var cipher = new CaesarCipherService();

        var result = cipher.Encrypt(plainText, shift, language);

        result.Should().Be(expectedCiphertext); // Compare exactly as-is.
    }

    [Theory]
    [InlineData("fbixg", "cześć", 5, "POLISH")]
    [InlineData("defąćę", "ąbćzac", 4, "POLISH")] 
    [InlineData("AĄ", "YZ", 4, "POLISH")] 
    public void Decrypt_ShouldReturnExpectedPlaintext_Polish(string cipherText, string expectedPlaintext,
        int shift, string language)
    {
        var cipher = new CaesarCipherService();

        var result = cipher.Decrypt(cipherText, shift, language);

        result.Should().Be(expectedPlaintext); 
    }
    
    
}
