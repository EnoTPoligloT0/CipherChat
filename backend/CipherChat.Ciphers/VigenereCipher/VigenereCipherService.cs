﻿using System;
using System.Text;

namespace CipherChat.Ciphers.VigenereCipher
{
    public class VigenereCipherService
    {
        private readonly string _alphabet;
        
        public VigenereCipherService(string language)
        {
            _alphabet = AlphabetProvider.GetAlphabet(language);
        }

        public string Encrypt(string plainText, string key)
        {
            return ProcessText(plainText, key, true);
        }

        public string Decrypt(string cipherText, string key)
        {
            return ProcessText(cipherText, key, false);
        }

        private string ProcessText(string text, string key, bool isEncryption)
        {
            StringBuilder result = new StringBuilder();
            key = key.ToLower();

            int keyIndex = 0;
            foreach (char character in text)
            {
                bool isUpper = char.IsUpper(character);
                char charToProcess = char.ToLower(character);

                if (_alphabet.Contains(charToProcess))
                {
                    int textCharPosition = _alphabet.IndexOf(charToProcess);
                    int keyCharPosition = _alphabet.IndexOf(key[keyIndex % key.Length]);

                    int newCharPosition = isEncryption
                        ? (textCharPosition + keyCharPosition) % _alphabet.Length
                        : (textCharPosition - keyCharPosition + _alphabet.Length) % _alphabet.Length;

                    char newChar = _alphabet[newCharPosition];
                    result.Append(isUpper ? char.ToUpper(newChar) : newChar);
                    keyIndex++;
                }
                else
                {
                    result.Append(character);
                }
            }

            return result.ToString();
        }

    }
}