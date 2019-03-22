using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTable
{
    class Program
    {
        static void Main(string[] args)
        {
            //HashTable tmp = new HashTable(5);
            HashTable lentele = new HashTable();
            lentele.Put("10", 55);

            //Random rnd = new Random(100);
            //for(int i = 0; i < 20; i++)
            //{
            //    tmp.Put(System.DateTime.Now.ToString(), rnd.NextDouble());
            //}

            //Console.WriteLine(tmp.ToString());

            Console.ReadKey();
        }
    }
}
