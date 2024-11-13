using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DSS
{
    public class DSS_Method
    {
        public static DSACryptoServiceProvider GenerateOrLoadKey(string PublicKeyFile, string PrivateKeyFile)
        {
            DSACryptoServiceProvider dsa;

            if (File.Exists(PrivateKeyFile) && File.Exists(PublicKeyFile))
            {
                try
                {
                    dsa = new DSACryptoServiceProvider();
                    string privateKeyXml = File.ReadAllText(PrivateKeyFile);
                    dsa.FromXmlString(privateKeyXml);
                    Console.WriteLine("Ключ завантажено з файлу.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка при завантаженні ключа: {ex.Message}");
                    throw;
                }
            }
            else
            {
                dsa = new DSACryptoServiceProvider();
                string privateKeyXml = dsa.ToXmlString(true); // Приватний ключ
                string publicKeyXml = dsa.ToXmlString(false); // Публічний ключ
                File.WriteAllText(PrivateKeyFile, privateKeyXml);
                File.WriteAllText(PublicKeyFile, publicKeyXml);
                Console.WriteLine("Ключі згенеровано та збережено у файли.");
            }

            return dsa;
        }

        public static byte[] SignData(string inputData, DSACryptoServiceProvider dsa)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(inputData);
            byte[] signedData = dsa.SignData(dataBytes);
            Console.WriteLine("Підпис створений для тексту.");
            return signedData;
        }

        public static byte[] SignFile(string filePath, DSACryptoServiceProvider dsa)
        {
            byte[] fileData = File.ReadAllBytes(filePath);
            byte[] signedData = dsa.SignData(fileData);
            Console.WriteLine("Підпис створений для файлу.");
            return signedData;
        }

        public static bool VerifySignature(byte[] data, byte[] signature, DSACryptoServiceProvider dsa)
        {
            return dsa.VerifyData(data, signature);
        }


    }
}
