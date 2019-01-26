using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Utils
{
    class Randoms
    {
        public static Random random = new Random();
        private static List<int> CreatedIDs = new List<int>();
        private static List<string> GeneratedHashes = new List<string>();

        public static int CreateRandomID()
        {
            int randomed = random.Next(1, 99999);

            if (!CreatedIDs.Contains(randomed))
            {
                CreatedIDs.Add(randomed);
                return randomed;
            }
            else
            {
                return CreateRandomID();
            }
        }

        public static string GenerateHash(int length)
        {
            const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var hash = new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            if (!GeneratedHashes.Contains(hash))
            {
                GeneratedHashes.Add(hash);
                return hash;
            }
            else
            {
                return GenerateHash(length);
            }
        }
    }
}
