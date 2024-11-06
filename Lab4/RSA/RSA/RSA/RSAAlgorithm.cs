using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace RSA.RCA
{
    public static class RSAAlgorithm
    {
        public static string GenerateAndSaveKeys(string publicKeyPath, string privateKeyPath)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;

                string publicKey = rsa.ToXmlString(false);
                File.WriteAllText(publicKeyPath, publicKey);

                string privateKey = rsa.ToXmlString(true);
                File.WriteAllText(privateKeyPath, privateKey);

                return "Ключі збережено у файлах: " + publicKeyPath + " та " + privateKeyPath + "\n";
            }
        }

        public static string EncryptFile(string inputFilePath, string outputFilePath, string publicKey)
        {
            byte[] fileData = File.ReadAllBytes(inputFilePath);
            byte[] encryptedData;

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.PersistKeyInCsp = false;
                rsa.FromXmlString(publicKey);
                Stopwatch sw = Stopwatch.StartNew();
                encryptedData = rsa.Encrypt(fileData, false);
                sw.Stop();
                File.WriteAllBytes(outputFilePath, encryptedData);
                return sw.Elapsed.TotalMilliseconds + "ms ";
            }

        }

        public static string DecryptFile(string inputFilePath, string outputFilePath, string privateKey)
        {
            byte[] encryptedData = File.ReadAllBytes(inputFilePath);
            byte[] decryptedData;

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.PersistKeyInCsp = false;
                rsa.FromXmlString(privateKey);
                Stopwatch sw = Stopwatch.StartNew();
                decryptedData = rsa.Decrypt(encryptedData, false);
                sw.Stop();
                File.WriteAllBytes(outputFilePath, decryptedData);
                return sw.Elapsed.TotalMilliseconds + "ms ";
            }

        }
    }
}
