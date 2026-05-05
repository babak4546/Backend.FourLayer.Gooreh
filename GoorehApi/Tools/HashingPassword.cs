using System.Security.Cryptography;
using System.Text;

namespace GoorehApi.Tools
{
    public static class HashingPassword
    {
        //public static string HashPssword(string password)
        //{
        //    using var sha=SHA512.Create();
        //    //tabdil beh bayte chon aksar systeam haye ramsnegarri ba string kar nemi konand
        //    var bytes = Encoding.UTF8.GetBytes(password);
        //    var makeHash= sha.ComputeHash(bytes);
        //    return Convert.ToBase64String(makeHash);
        //}
        public static string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        // هش کردن پسورد با Salt + Pepper
        public static string HashPassword(string password, string salt, string pepper)
        {
            var combined = password + salt + pepper;

            using (var sha = SHA512.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(combined);
                var hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}


