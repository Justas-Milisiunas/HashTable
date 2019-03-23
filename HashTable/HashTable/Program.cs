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
        static void Main(string[] args)
        {
            int n = 10000000;
            int seed = 121;
            string dataFile = "data-file.txt";
            string[] keys = new string[n];

            keys = GenerateDataFile(dataFile, n, seed);
            TestHashTable_D(dataFile, n, seed, keys);
            //TestHashTable_OP(dataFile, n, seed, keys);

            Console.ReadKey();
        }

        public static void TestHashTable_OP(string dataFile, int n, int seed, string[] keys)
        {
            //Reads data from file
            HashTable lentele = new HashTable();
            using (StreamReader reader = new StreamReader(dataFile))
            {
                int i = 0;
                string line = null;
                while((line = reader.ReadLine()) != null)
                {
                    string[] words = line.Split(' ');
                    lentele.Put(words[0], double.Parse(words[1]));
                    keys[i++] = words[0];
                }
            }

            int sum = 0;
            Stopwatch t1 = new Stopwatch();
            t1.Start();
            for (int i = 0; i < n; i++)
            {
                if (lentele.Contains(keys[i]))
                    sum++;
            }
            t1.Stop();

            Console.WriteLine(sum == n ? "Rado visus" : "Visu nerado");
            Console.WriteLine("Uztruko: " + t1.Elapsed);

            lentele = null;
            keys = null;
            GC.Collect();
        }

        public static void TestHashTable_D(string dataFile, int n, int seed, string[] keys)
        {
            HashTableD table = new HashTableD();
            ReadData(dataFile, table);

            int sum = 0;
            for(int i = 0; i < n; i++)
            {
                if (table.Contains(keys[i]))
                    sum++;
            }

            Console.WriteLine(table.ToString());
            Console.WriteLine("Rasta: " + sum);

            while(true)
            {
                string input = Console.ReadLine();
                Console.WriteLine("Value: " + table.Get(input));
            }
        }

        public static void ReadData(string dataFile, HashTableD table)
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
                //writer.WriteLine(12345678 + " " + rand.NextDouble());
                //writer.WriteLine(11345678 + " " + rand.NextDouble());
                //writer.WriteLine(11145678 + " " + rand.NextDouble());
                //writer.WriteLine(11115678 + " " + rand.NextDouble());
                //writer.WriteLine(11115678 + " " + 111);
                //writer.WriteLine(11111678 + " " + rand.NextDouble());
                //writer.WriteLine(11111678 + " " + rand.NextDouble());
                //writer.WriteLine(11111678 + " " + rand.NextDouble());
                //writer.WriteLine(11111678 + " " + rand.NextDouble());
                //writer.WriteLine(11111678 + " " + rand.NextDouble());
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
