using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DSS
{
    internal class Handler
    {
        private const string PrivateKeyFile = "private_key.txt";
        private const string PublicKeyFile = "public_key.txt";
        public static string SignFile(string filePath, string fileSign)
        {    
            DSACryptoServiceProvider dsa = DSS_Method.GenerateOrLoadKey(PublicKeyFile, PrivateKeyFile);
            byte[] signature = DSS_Method.SignFile(filePath, dsa);
            File.WriteAllBytes(fileSign, signature);
            return "Підпис у шестнадцятковому форматі для файлу:\n" + 
            BitConverter.ToString(signature).Replace("-", "") + "\n";
        }

        public static string SignText(string text, string textSign)
        {
            DSACryptoServiceProvider dsa = DSS_Method.GenerateOrLoadKey(PublicKeyFile, PrivateKeyFile);
            byte[] signature = DSS_Method.SignData(text, dsa);
            File.WriteAllBytes(textSign, signature);
            return "Підпис у шестнадцятковому форматі:\n" +
            BitConverter.ToString(signature).Replace("-", "") + "\n";
        }

        public static string VerifyFile(string fileToVerifyPath, string signatureFilePath)
        {
            byte[] fileData = File.ReadAllBytes(fileToVerifyPath);
            byte[] fileSignature = File.ReadAllBytes(signatureFilePath);
            if (fileSignature.Length != 40)
            {
                return "Підпис має неправильну довжину. Очікується 40 байт. \" + fileSignature.Length\n";
            }
            var verifyDsa = new DSACryptoServiceProvider();
            string publicKeyXml = File.ReadAllText(PublicKeyFile);
            verifyDsa.FromXmlString(publicKeyXml);

            bool isVerified = DSS_Method.VerifySignature(fileData, fileSignature, verifyDsa);
            return isVerified ? "Підпис файлу дійсний.\n" : "Підпис файлу недійсний.\n";
        }

        public static string VerifyText(string textToVerify, string signatureTextPath)
        {
            byte[] data = Encoding.UTF8.GetBytes(textToVerify);
            byte[] fileSignature = File.ReadAllBytes(signatureTextPath);
            if (fileSignature.Length != 40)
            {
                return "Підпис має неправильну довжину. Очікується 40 байт. \" + fileSignature.Length\n";
            }
            var verifyDsa = new DSACryptoServiceProvider();
            string publicKeyXml = File.ReadAllText(PublicKeyFile);
            verifyDsa.FromXmlString(publicKeyXml);

            bool isVerified = DSS_Method.VerifySignature(data, fileSignature, verifyDsa);
            return isVerified ? "Підпис тексту дійсний.\n" : "Підпис тексту недійсний.\n";
        }

    }
}
