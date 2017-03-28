using System;
using System.Security.Cryptography;
using System.Text;

namespace WebService
{
    public static class Password
    {
        private static Random random;

        static Password()
        {
            Password.random = new Random();
        }

        public static byte[] CreateRandomSalt()
        {
            byte[] s = new byte[4];
            RandomNumberGenerator.Create().GetBytes(s);
            return s;
        }

        public static byte[] CreateSHA512Hash(string str, byte[] salt)
        {
            byte[] input = (new UTF32Encoding()).GetBytes(str);
            byte[] combined = new byte[(int)input.Length + (int)salt.Length];
            Array.Copy(input, combined, (int)input.Length);
            Array.Copy(salt, 0, combined, (int)input.Length, (int)salt.Length);
            SHA512 hash = SHA512.Create();
            hash.ComputeHash(combined);
            return hash.Hash;
        }

        public static int RandomInt(int nMin, int nMax)
        {
            return Password.random.Next(nMin, nMax + 1);
        }
    }
}