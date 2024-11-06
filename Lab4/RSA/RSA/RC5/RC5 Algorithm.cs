using System;
using System.Diagnostics;
using System.IO;


namespace RC5
{
    internal class RC5_Algorithm
    {
        private const int W = 32; 
        private const int R = 20; 
        private const int B = 16;   

        private uint[] S = Array.Empty<uint>(); 
        private uint[] L = Array.Empty<uint>();

        private const uint P = 0xB7E15163;
        private const uint Q = 0x9E3779B9;

       
        public RC5_Algorithm(byte[] key)
        {
            KeyExpansion(key);
            
        }
        public RC5_Algorithm(string key)
        {
            KeyExpansion(MD5_Algorithm.ComputeMD5(key));
        }

        private void KeyExpansion(byte[] key)
        {
            int c = (key.Length + 3) / 4;
            L = new uint[c];
            for (int i = 0; i < key.Length; i++)
            {
                L[i / 4] |= (uint)(key[i] << (8 * (i % 4)));
            }

            S = new uint[2 * R + 2];
            S[0] = P;
            for (int i = 1; i < S.Length; i++)
            {
                S[i] = S[i - 1] + Q;
            }

            uint A = 0, B = 0;
            int k = 0, j = 0;
            int max = Math.Max(3 * c, 2 * R + 2);
            for (int s = 0; s < max; s++)
            {
                A = S[k] = RotateLeft(S[k] + A + B, 3);
                B = L[j] = RotateLeft(L[j] + A + B, (int)(A + B));
                k = (k + 1) % S.Length;
                j = (j + 1) % L.Length;
            }
        }

        public byte[] Encrypt(byte[] plaintext)
        {
            uint[] data = new uint[2];
            Buffer.BlockCopy(plaintext, 0, data, 0, 8);
            data[0] += S[0];

            for (int i = 1; i <= R; i++)
            {
                data[1] ^= RotateLeft(data[0], (int)data[0]) + S[i];
                data[0] ^= RotateLeft(data[1], (int)data[1]) + S[i + R];
            }

            byte[] ciphertext = new byte[8];
            Buffer.BlockCopy(data, 0, ciphertext, 0, 8);
            return ciphertext;
        }
        
        public byte[] Decrypt(byte[] ciphertext)
        {
            uint[] data = new uint[2];
            Buffer.BlockCopy(ciphertext, 0, data, 0, 8);

            for (int i = R; i > 0; i--)
            {
                data[0] ^= RotateLeft(data[1], (int)data[1]) + S[i + R];
                data[1] ^= RotateLeft(data[0], (int)data[0]) + S[i];
            }

            data[0] -= S[0];
            byte[] plaintext = new byte[8];
            Buffer.BlockCopy(data, 0, plaintext, 0, 8);
            return plaintext;
        }
        private static uint RotateLeft(uint x, int y)
        {
            return (x << y) | (x >> (W - y));
        }

      
        public byte[] EncryptCBC(byte[] plaintext, byte[] iv)
        {
            byte[] ciphertext = new byte[plaintext.Length];
            byte[] block = new byte[8];

            for (int i = 0; i < plaintext.Length; i += 8)
            {
                
                for (int j = 0; j < 8 && (i + j) < plaintext.Length; j++)
                {
                    block[j] = (i == 0) ? (byte)(plaintext[i + j] ^ iv[j]) : (byte)(plaintext[i + j] ^ ciphertext[i - 8 + j]);
                }

                byte[] encryptedBlock = Encrypt(block);
                Buffer.BlockCopy(encryptedBlock, 0, ciphertext, i, 8);
            }

            return ciphertext;
        }

        public byte[] DecryptCBC(byte[] ciphertext, byte[] iv)
        {
            byte[] plaintext = new byte[ciphertext.Length];
            byte[] block = new byte[8];

            for (int i = 0; i < ciphertext.Length; i += 8)
            {
                byte[] decryptedBlock = Decrypt(ciphertext, i);
                for (int j = 0; j < 8 && (i + j) < ciphertext.Length; j++)
                {
                    block[j] = (i == 0) ? (byte)(decryptedBlock[j] ^ iv[j]) : (byte)(decryptedBlock[j] ^ ciphertext[i - 8 + j]);
                }
                Buffer.BlockCopy(block, 0, plaintext, i, 8);
            }

            return plaintext;
        }

        private byte[] Decrypt(byte[] ciphertext, int offset)
        {
            uint[] data = new uint[2];
            Buffer.BlockCopy(ciphertext, offset, data, 0, 8);

            for (int i = R; i > 0; i--)
            {
                data[0] ^= RotateLeft(data[1], (int)data[1]) + S[i + R];
                data[1] ^= RotateLeft(data[0], (int)data[0]) + S[i];
            }

            data[0] -= S[0];
            byte[] plaintext = new byte[8];
            Buffer.BlockCopy(data, 0, plaintext, 0, 8);
            return plaintext;
        }

        public string EncryptFile(string inputFilePath, string outputFilePath, byte[] iv)
        {
            byte[] plaintext = File.ReadAllBytes(inputFilePath);
            plaintext = Pad(plaintext);
            Stopwatch sw = Stopwatch.StartNew();
            byte[] ciphertext = EncryptCBC(plaintext, iv);
            sw.Stop();
            using (var fileStream = new FileStream(outputFilePath, FileMode.Create))
            {
                fileStream.Write(iv, 0, iv.Length);
                fileStream.Write(ciphertext, 0, ciphertext.Length);
            }
            return sw.Elapsed.TotalMilliseconds + "ms ";
        }


        public string DecryptFile(string inputFilePath, string outputFilePath)
        {
            byte[] fileContent = File.ReadAllBytes(inputFilePath);

            int ivLength = 8;
            byte[] iv = new byte[ivLength];
            byte[] ciphertext = new byte[fileContent.Length - ivLength];

            Array.Copy(fileContent, 0, iv, 0, ivLength);
            Array.Copy(fileContent, ivLength, ciphertext, 0, ciphertext.Length);
            Stopwatch sw = Stopwatch.StartNew();
            byte[] decryptedText = DecryptCBC(ciphertext, iv);
            sw.Stop();
            File.WriteAllBytes(outputFilePath, decryptedText);
            return sw.Elapsed.TotalMilliseconds + "ms ";
        }


        public static byte[] Pad(byte[] input)
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
