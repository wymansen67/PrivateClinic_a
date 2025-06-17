using System;
using System.Linq;
using System.Security.Cryptography;

namespace AvaloniaPrivateClinic.Utility;

public static class PasswordEncryption
{
    //512 + 512 = 1 0 2 4 bit Anya destroys in millisecond
    private const int SaltSize = 64; // 512 bits
    private const int KeySize = 64; // 512 bits
    private const int Iterations = 100_000; // Increased iteration count for better security

    public static (string Hash, string Salt) HashPassword(string password)
    {
        using var algorithm = new Rfc2898DeriveBytes(
            password,
            SaltSize,
            Iterations,
            HashAlgorithmName.SHA512);
        var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
        var salt = Convert.ToBase64String(algorithm.Salt);

        return (key, salt);
    }

    public static bool VerifyPassword(string password, string hash, string salt)
    {
        var saltBytes = Convert.FromBase64String(salt);
        var keyBytes = Convert.FromBase64String(hash);

        using var algorithm = new Rfc2898DeriveBytes(
            password,
            saltBytes,
            Iterations,
            HashAlgorithmName.SHA512);
        var keyToCheck = algorithm.GetBytes(KeySize);

        return keyToCheck.SequenceEqual(keyBytes);
    }
}