﻿using System.Security.Cryptography;
using Application.Features.Authentication.Interfaces;

namespace Application.Features.Authentication
{
    public class SecretHasher : ISecretHasher
    {
        // 128 bits
        private const int SaltSize = 16;
        // 256 bits
        private const int KeySize = 32;
        private const int Iterations = 50000;
        private static readonly HashAlgorithmName _algorithm = HashAlgorithmName.SHA256;
        private const char SegmentDelimiter = ':';

        public string Hash(string input)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                input,
                salt,
                Iterations,
                _algorithm,
                KeySize
            );
            return string.Join(
                SegmentDelimiter,
                Convert.ToHexString(hash),
                Convert.ToHexString(salt),
                Iterations,
                _algorithm
            );
        }

        public bool Verify(string input, string hashString)
        {
            string[] segments = hashString.Split(SegmentDelimiter);
            byte[] hash = Convert.FromHexString(segments[0]);
            byte[] salt = Convert.FromHexString(segments[1]);
            int iterations = int.Parse(segments[2]);
            HashAlgorithmName algorithm = new(segments[3]);
            byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(
                input,
                salt,
                iterations,
                algorithm,
                hash.Length
            );
            return CryptographicOperations.FixedTimeEquals(inputHash, hash);
        }
    }
}
