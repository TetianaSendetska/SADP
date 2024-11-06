using System;
using System.Security.Cryptography;
using System.IO;
using RSA.RCA;
using RC5;
using System.Text;
using System.Windows;
using System.Diagnostics;

namespace RSA
{
    internal class Handler
    {
        public static string EncryptRCA(string originalFilePath, string encryptedFilePath, string publicKeyPath, string privateKeyPath)
        {
            string keys = RSAAlgorithm.GenerateAndSaveKeys(publicKeyPath, privateKeyPath);
     
            string publicKey = File.ReadAllText(publicKeyPath);
            string privateKey = File.ReadAllText(privateKeyPath);

            string time = RSAAlgorithm.EncryptFile(originalFilePath, encryptedFilePath, publicKey);
            return keys + time + "Файл зашифровано та збережено у: " + encryptedFilePath+ "\n";
        }
        public static string DecryptRCA(string encryptedFilePath, string decryptedFilePath, string privateKeyPath)
        {
            string privateKey = File.ReadAllText(privateKeyPath);

            string time = RSAAlgorithm.DecryptFile(encryptedFilePath, decryptedFilePath, privateKey);
            return time + "Файл розшифровано та збережено у: " + decryptedFilePath + "\n";
        }

        public static string GenerateEnc(string key, string filepath, string output, byte[] iv)
        {
            byte[] keyByte = Encoding.UTF8.GetBytes(key);

            RC5_Algorithm rc5 = new RC5_Algorithm(keyByte);
            string time = rc5.EncryptFile(filepath, output, iv);
            return time + "Шифрування файлу виконано\n";
        }
        public static string GenerateDec(string key, string filepath, string decoutput)
        {
            byte[] keyByte = Encoding.UTF8.GetBytes(key);

            RC5_Algorithm rc5 = new RC5_Algorithm(keyByte);
            string time = rc5.DecryptFile(filepath, decoutput);
            return time + "Дешифрування файлу виконано\n";
        }

        static byte[] Pad(byte[] input)
        {
            int padSize = 8 - (input.Length % 8);
            Array.Resize(ref input, input.Length + padSize);
            for (int i = input.Length - padSize; i < input.Length; i++)
            {
                input[i] = (byte)padSize;
            }
            return input;
        }
    }
}
