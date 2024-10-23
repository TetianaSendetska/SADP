using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC5
{
    internal class Rand_Generator
    {
        private readonly int m;
        private readonly int a;
        private readonly int c;
        private int x;

        public Rand_Generator(int m, int a, int c, int X0)
        {
            this.m = m;
            this.a = a;
            this.c = c;
            x = X0;
        }

        public int Next()
        {
            x = (a * x + c) % m;
            return x;
        }
    }
}
