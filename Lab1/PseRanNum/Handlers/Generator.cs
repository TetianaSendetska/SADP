namespace PseRanNum.Handlers
{
    public static class Generator
    {
        static public long[] Generate(long i, long m, long a, long c, long x0)
        {
            long[] result = new long[i];

            for (int j = 0; j < i; ++j)
            {
                result[j] = ((a * x0) + c) % m;
                x0 = result[j];
            }

            return result;
        }

        static public int CheckPeriod(long[] array)
        {
            Dictionary<long, int> seenNumbers = new Dictionary<long, int>();

            for (int i = 0; i < array.Length; i++)
            {
                long number = array[i];

                if (seenNumbers.TryGetValue(number, out int firstIndex))
                {
                    return i;
                }

                seenNumbers[number] = i;
            }

            return -1;
        }
        
    }
}
