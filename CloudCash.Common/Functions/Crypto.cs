using System.Linq;
using System.Security.Cryptography;

namespace CloudCash.Common.Functions
{
    public class Crypto
    {
        private static (byte[] Salt, byte[] Hash) GetHash(string pass, byte[] salt = null)
        {
            byte[] saltByte;

            if (salt is null)
            {
                saltByte = new byte[8];
                using RNGCryptoServiceProvider rngCsp = new();
                rngCsp.GetBytes(saltByte);
            }
            else
                saltByte = salt;

            Rfc2898DeriveBytes hash = new(pass, saltByte);
            hash.Reset();
            return (saltByte, hash.GetBytes(256));
        }

        public static bool Decrypt(byte[] salt, byte[] hash, string pass) => Enumerable.SequenceEqual(GetHash(pass, salt).Hash, hash);

        public static (byte[] Salt, byte[] Hash) Encrypt(string pass) => GetHash(pass);
    }
}
