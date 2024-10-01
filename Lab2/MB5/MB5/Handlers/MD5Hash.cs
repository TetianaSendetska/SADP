using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MB5.Handlers
{
    internal class MD5Hash
    {
        public static string GetMd5Hash(byte[] input)
        {
            byte[] hash = Algorithm.ComputeMD5(input);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        public static string GetMd5HashFromFile(string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] fileBytes = new byte[fileStream.Length];
                fileStream.Read(fileBytes, 0, fileBytes.Length);

                string hash = GetMd5Hash(fileBytes);
                return hash;
            }
        }
    }
}
