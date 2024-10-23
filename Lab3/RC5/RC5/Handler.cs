using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RC5
{
    internal static class Handler
    {
        public static (string, string) Generate(string key, string message)
        {
            byte[] keyByte = Encoding.UTF8.GetBytes(key);

            byte[] iv = IV_Generator.GenerateIV();

            RC5_Algorithm rc5 = new RC5_Algorithm(key);
           
            byte[] plaintext = Encoding.UTF8.GetBytes(message);
            plaintext = Pad(plaintext);

            byte[] ciphertext = rc5.EncryptCBC(plaintext, iv);
            byte[] decryptedText = rc5.DecryptCBC(ciphertext, iv);

            return (BitConverter.ToString(ciphertext), Encoding.UTF8.GetString(decryptedText).TrimEnd('\0'));

        }

        public static void Generate(string key, string filepath, string output, string decoutput)
        {
            byte[] keyByte = Encoding.UTF8.GetBytes(key);
            byte[] iv = new byte[8]; 
            new Random().NextBytes(iv); 

            RC5_Algorithm rc5 = new RC5_Algorithm(keyByte);

            rc5.EncryptFile(filepath, output, iv);

            rc5.DecryptFile(output, decoutput, iv);

            MessageBox.Show("Шифрування і дешифрування файлу виконано", "єєєй", MessageBoxButton.OK,
                                          MessageBoxImage.Information);
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
