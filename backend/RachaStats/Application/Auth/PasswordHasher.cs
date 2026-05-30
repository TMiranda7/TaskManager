using System.Security.Cryptography;
using System.Text;

namespace RachaStats.Application.Auth;

public static class PasswordHasher
{
    public static string ComputeSha256(string password)
    {
        var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));

        return Convert.ToHexString(hashBytes);
    }
}
