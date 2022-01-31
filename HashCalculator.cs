using System;
using System.Security.Cryptography;
using System.Text;

namespace FileSignature
{
    internal class HashCalculator
    {
        private readonly SHA256 _sha256;
        public HashCalculator(SHA256 sha256)
        {
            _sha256 = sha256;
        }

        /// <summary>
        /// Calculates a hash and returns it as a string
        /// </summary>
        public string GetHashString(byte[] buffer, int length)
        {
            var hash = _sha256.ComputeHash(buffer, 0, length);
            var builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
