﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace RC5
{
    internal class MD5_Algorithm
    {
        private static readonly uint[] T = new uint[64]
   {
        0xd76aa478, 0xe8c7b756, 0x242070db, 0xc1bdceee, 0xf57c0faf, 0x4787c62a, 0xa8304613, 0xfd469501,
        0x698098d8, 0x8b44f7af, 0xffff5bb1, 0x895cd7be, 0x6b901122, 0xfd987193, 0xa679438e, 0x49b40821,
        0xf61e2562, 0xc040b340, 0x265e5a51, 0xe9b6c7aa, 0xd62f105d, 0x02441453, 0xd8a1e681, 0xe7d3fbc8,
        0x21e1cde6, 0xc33707d6, 0xf4d50d87, 0x455a14ed, 0xa9e3e905, 0xfcefa3f8, 0x676f02d9, 0x8d2a4c8a,
        0xfffa3942, 0x8771f681, 0x6d9d6122, 0xfde5380c, 0xa4beea44, 0x4bdecfa9, 0xf6bb4b60, 0xbebfbc70,
        0x289b7ec6, 0xeaa127fa, 0xd4ef3085, 0x04881d05, 0xd9d4d039, 0xe6db99e5, 0x1fa27cf8, 0xc4ac5665,
        0xf4292244, 0x432aff97, 0xab9423a7, 0xfc93a039, 0x655b59c3, 0x8f0ccc92, 0xffeff47d, 0x85845dd1,
        0x6fa87e4f, 0xfe2ce6e0, 0xa3014314, 0x4e0811a1, 0xf7537e82, 0xbd3af235, 0x2ad7d2bb, 0xeb86d391
   };

        private static readonly int[] S = new int[64]
        {
        7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22,
        5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20,
        4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23,
        6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21
        };

        private static uint RotateLeft(uint x, int n)
        {
            return x << n | x >> 32 - n;
        }

        private static void ProcessBlock(ref uint[] buffer, byte[] block)
        {
            uint[] X = new uint[16];
            for (int i = 0; i < 16; i++)
            {
                X[i] = BitConverter.ToUInt32(block, i * 4);
            }

            uint a = buffer[0];
            uint b = buffer[1];
            uint c = buffer[2];
            uint d = buffer[3];

            for (int i = 0; i < 64; i++)
            {
                uint f = 0, g = 0;

                if (i < 16)
                {
                    f = b & c | ~b & d;
                    g = (uint)i;
                }
                else if (i < 32)
                {
                    f = b & d | c & ~d;
                    g = (uint)((5 * i + 1) % 16);
                }
                else if (i < 48)
                {
                    f = b ^ c ^ d;
                    g = (uint)((3 * i + 5) % 16);
                }
                else
                {
                    f = c ^ (b | ~d);
                    g = (uint)(7 * i % 16);
                }

                uint temp = d;
                d = c;
                c = b;
                b = b + RotateLeft(a + f + T[i] + X[g], S[i]);
                a = temp;
            }

            buffer[0] += a;
            buffer[1] += b;
            buffer[2] += c;
            buffer[3] += d;
        }

        public static byte[] ComputeMD5(string text)
        {
            byte[] input = Encoding.UTF8.GetBytes(text);
            uint[] buffer = new uint[4]
            {
            0x67452301,
            0xefcdab89,
            0x98badcfe,
            0x10325476
            };

            int originalLength = input.Length;
            int paddingLength = (56 - (originalLength + 1) % 64 + 64) % 64;

            byte[] paddedInput = new byte[originalLength + paddingLength + 1 + 8];

            Array.Copy(input, paddedInput, originalLength);
            paddedInput[originalLength] = 0x80;
            BitConverter.GetBytes((ulong)originalLength * 8).CopyTo(paddedInput, paddedInput.Length - 8);

            for (int i = 0; i < paddedInput.Length / 64; i++)
            {
                byte[] block = new byte[64];
                Array.Copy(paddedInput, i * 64, block, 0, 64);
                ProcessBlock(ref buffer, block);
            }

            return buffer.SelectMany(BitConverter.GetBytes).ToArray();
        }
    }
}
