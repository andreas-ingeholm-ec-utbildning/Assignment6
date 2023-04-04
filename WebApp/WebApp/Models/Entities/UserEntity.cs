using System.Security.Cryptography;
using System.Text;

namespace WebApp.Models.Entities;

public class UserEntity
{

    public Guid ID { get; set; } = Guid.NewGuid();
    public string EmailAddress { get; set; } = null!;

    public byte[] Password { get; private set; } = null!;
    public byte[] SecurityKey { get; private set; } = null!;

    public void GeneratePassword(string password)
    {
        using var hmac = new HMACSHA512();
        SecurityKey = hmac.Key;
        Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    public bool ValidatePassword(string password)
    {

        using var hmac = new HMACSHA512(SecurityKey);
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        if (hash.Length != Password.Length)
            return false;

        for (int i = 0; i < hash.Length; i++)
        {
            if (hash[i] != Password[i])
                return false;
        }

        return true;

    }

}
