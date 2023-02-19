using Konscious.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Argo2
{
    class Program
    {
        // No. of CPU Cores x 2.
        private const int DEGREE_OF_PARALLELISM = 16;

        // Recommended minimum value.
        private const int NUMBER_OF_ITERATIONS = 4;

        // 600 MB.
        private const int MEMORY_TO_USE_IN_KB = 600000;

        public static void Main(string[] args)
        {
            var password = "SomeSecurePassword";
            byte[] salt = CreateSalt();
            byte[] hash = HashPassword(password, salt);

            string pahtCreate = "C:\\Users\\Rodrigo\\Documents\\Curso C#\\Argo2\\Argo2\\Example";
            
            Directory.CreateDirectory(pahtCreate);

            string filePath = Path.Combine(pahtCreate, "MiArchivo.txt");

            using (StreamWriter writer = File.CreateText(filePath))
            {
                writer.WriteLine("Ejemlo");
                writer.WriteLine("Salt: " + Convert.ToBase64String(salt));
                writer.WriteLine("Hash: " + Convert.ToBase64String(hash));
            }


            
        
        }

        private static byte[] CreateSalt()
        {
            var buffer = new byte[16];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(buffer);

            return buffer;
        }

        private static byte[] HashPassword(string password, byte[] salt)
        {
            var argon2id = new Argon2id(Encoding.UTF8.GetBytes(password));
            argon2id.Salt = salt;
            argon2id.DegreeOfParallelism = DEGREE_OF_PARALLELISM;
            argon2id.Iterations = NUMBER_OF_ITERATIONS;
            argon2id.MemorySize = MEMORY_TO_USE_IN_KB;

            return argon2id.GetBytes(16);

        }

        private static bool VerifyHash(string password, byte[] salt, byte[] hash)
        {
            var newHash = HashPassword(password, salt);
            return hash.SequenceEqual(newHash);
        }


    }
}
