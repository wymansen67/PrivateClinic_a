using System;
using System.Security.Cryptography;
using System.Text;

namespace AvaloniaPrivateClinic.Encryption;

public static class Cryptography
{
    private const string Key = "AVHBelovedWifey";
    private const int KeySize = 32;
    private const int IvSize = 12;
    private const int TagSize = 16;

    public static string? Encrypt(string? plainText)
    {
        try
        {
            if (plainText != null)
            {
                var keyBytes = GetKey(Key);
                var iv = new byte[IvSize];

                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(iv);
                }

                using (var aes = new AesGcm(keyBytes, TagSize))
                {
                    var plainBytes = Encoding.UTF8.GetBytes(plainText);
                    var cipherBytes = new byte[plainBytes.Length];
                    var tag = new byte[TagSize];

                    aes.Encrypt(iv, plainBytes, cipherBytes, tag);

                    var encryptedData = new byte[IvSize + cipherBytes.Length + TagSize];
                    Array.Copy(iv, 0, encryptedData, 0, IvSize);
                    Array.Copy(cipherBytes, 0, encryptedData, IvSize, cipherBytes.Length);
                    Array.Copy(tag, 0, encryptedData, IvSize + cipherBytes.Length, TagSize);

                    return Convert.ToBase64String(encryptedData);
                }
            }

            return null!;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static string? Decrypt(string? cipherText)
    {
        try
        {
            if (cipherText != null)
            {
                var keyBytes = GetKey(Key);

                cipherText = cipherText.PadRight(cipherText.Length + (4 - cipherText.Length % 4) % 4, '=');
                var cipherData = Convert.FromBase64String(cipherText);

                var iv = new byte[IvSize];
                var tag = new byte[TagSize];
                var cipherBytes = new byte[cipherData.Length - IvSize - TagSize];

                Array.Copy(cipherData, 0, iv, 0, IvSize);
                Array.Copy(cipherData, IvSize, cipherBytes, 0, cipherBytes.Length);
                Array.Copy(cipherData, IvSize + cipherBytes.Length, tag, 0, TagSize);

                using (var aes = new AesGcm(keyBytes, TagSize))
                {
                    var plainBytes = new byte[cipherBytes.Length];
                    aes.Decrypt(iv, cipherBytes, tag, plainBytes);
                    return Encoding.UTF8.GetString(plainBytes);
                }
            }

            return null!;
        }
        catch (Exception)
        {
            return null;
        }
    }


    private static byte[] GetKey(string key)
    {
        using (var sha256 = SHA256.Create())
        {
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
        }
    }
}