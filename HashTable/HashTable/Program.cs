using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace HashTable
{
    class Program
    {
        public static readonly int[] KIEKIAI = new int[] { 250, 750, 2500, 10_000, 50_000, 150_000, 500_000 };

        static void Main(string[] args)
        {
            int seed = (int)DateTime.Now.Ticks & 0x0000FFFF;

            while(true)
            {
                Console.WriteLine("Ka norite istestuoti?");
                Console.WriteLine("hash_op hash_d all exit");

                string input = Console.ReadLine();
                if (input.ToLower() == "hash_op")
                    TestHashTable_OP(seed);
                else if (input.ToLower() == "hash_d")
                    TestHashTable_D(seed);
                else if (input.ToLower() == "all")
                {
                    TestHashTable_OP(seed);
                    TestHashTable_D(seed);
                }
                else
                    break;
            }
        }

        public static void TestHashTable_OP(int seed)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("HASHTABLE OP");
            builder.AppendLine("=============================================================================");
            builder.AppendLine("| Number of elements |        |    Runtime time   |        |   Operations   |");
            builder.AppendLine("===================================HASHTABLE-OP==============================");

            foreach (int count in KIEKIAI)
            {
                HashTableInt lentele = new HashTable();
                string[] keys = GenerateDataFile(count.ToString() + ".txt", count, seed);
                ReadData(count.ToString() + ".txt", lentele);

                //int sum = 0;
                Stopwatch t1 = new Stopwatch();
                t1.Start();
                for (int i = 0; i < count; i++)
                {
                    lentele.Contains(keys[i]);
                    //if(lentele.Contains(keys[i]))
                        //sum++;
                }
                t1.Stop();

                //Console.WriteLine("Rado: " + sum);
                //Console.WriteLine("Uztruko: " + t1.Elapsed);
                builder.AppendLine(string.Format("|{0,-20}|        |{1} ms|        |{2,-16}|", count, t1.Elapsed.ToString(), lentele.operationsCount));
                //Console.WriteLine(lentele.ToString());
                lentele = null;
                GC.Collect();
            }

            builder.AppendLine("=============================================================================");
            Console.Write(builder.ToString());
        }

        public static void TestHashTable_D(int seed)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("HASHTABLE D");
            builder.AppendLine("=============================================================================");
            builder.AppendLine("| Number of elements |        |    Runtime time   |        |   Operations   |");
            builder.AppendLine("===================================HASHTABLE-D===============================");

            foreach(int count in KIEKIAI)
            {
                HashTableInt table = new HashTableD();
                string[] keys = GenerateDataFile(count.ToString() + ".txt", count, seed);
                ReadData(count.ToString() + ".txt", table);

                //int sum = 0;
                Stopwatch t1 = new Stopwatch();
                t1.Start();
                for (int i = 0; i < count; i++)
                {
                    table.Contains(keys[i]);
                    //if (table.Contains(keys[i]))
                        //sum++;
                }
                t1.Stop();

                //Console.WriteLine("Rado: " + sum);
                //Console.WriteLine("Uztruko: " + t1.Elapsed);
                builder.AppendLine(string.Format("|{0,-20}|        |{1} ms|        |{2,-16}|", count, t1.Elapsed.ToString(), table.operationsCount));

                table = null;
                GC.Collect();
            }

            builder.AppendLine("=============================================================================");
            Console.Write(builder.ToString());
        }

        public static void ReadData(string dataFile, HashTableInt table)
        {
            using (StreamReader reader = new StreamReader(dataFile))
            {
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] words = line.Split(' ');
                    table.Put(words[0], double.Parse(words[1]));
                }
            }
        }

        public static string[] GenerateDataFile(string fileName, int n, int seed)
        {
            string[] keys = new string[n];
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                Random rand = new Random(seed);
                for (int i = 0; i < n; i++)
                {
                    string key = rand.Next(10000000, 99999999).ToString();
                    keys[i] = key;
                    writer.WriteLine(key + " " + rand.NextDouble());
                }
            }

            return keys;
        }
    }
}
