using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC5
{
    internal static class IV_Generator
    {
        public static byte[] GenerateIV()
        {

            int m = 256;
            int a = 1103515245;
            int c = 12345;
            int X0 = new Random().Next(m);

            Rand_Generator randGen = new Rand_Generator(m, a, c, X0);

            byte[] iv = new byte[8];
            for (int i = 0; i < iv.Length; i++)
            {
                iv[i] = (byte)(randGen.Next() % 256);
            }

            return iv;
        }
    }
}
