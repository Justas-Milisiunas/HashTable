using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HashTable
{
    public class HashTable
    {
        private readonly double loadFactor = 0.75;

        private Entry[] table;

        private int size;
        private int capacity;
        private int chainsCount;

        public HashTable(int capacity = 10)
        {
            this.capacity = capacity;
            this.table = new Entry[capacity];
            this.size = 0;
            this.chainsCount = 0;
        }

        public double Put(string key, double value)
        {
            if(key == null)
            {
                throw new ArgumentException("Key or value is null in Put(string key, double value)");
            }

            int foundIndex = FindPosition(key);
            if(foundIndex == -1)
            {
                Rehash(capacity * 2);
                Put(key, value);
            }
            else
            {
                table[foundIndex] = new Entry(key, value);
                size++;

                if (size > capacity * loadFactor)
                    Rehash(capacity * 2);
            }

            return value;
        }

        public double? Get(string key)
        {
            if (key == null)
                throw new ArgumentNullException("Key is null in Get(string key)");

            int index = Hash(key);
            int tempIndex = index;
            int j = 0;
            for(int i = 0; i < table.Length; i++)
            {
                if (table[index] == null)
                    return null;
                if (table[index].key.Equals(key))
                    return table[index].value;
                j++;
                index = (tempIndex + j) % table.Length;
            }
            return null;
        }

        public bool Contains(string key)
        {
            return Get(key) == null ? false : true;
        }

        private int FindPosition(string key)
        {
            if (key == null)
                throw new ArgumentNullException("Key is null");

            int index = Hash(key);
            int tempIndex = index;
            int i = 0;

            for(int j = 0; j < table.Length; j++)
            {
                if(table[index] == null || table[index].key.Equals(key))
                {
                    return index;
                }
                i++;
                index = (tempIndex + i) % table.Length;
            }

            return -1;
        }

        private void Rehash(int capacity)
        {
            HashTable newHT = new HashTable(capacity);
            for(int i = 0; i < table.Length; i++)
            {
                if(table[i] != null)
                    newHT.Put(table[i].key, table[i].value);
            }

            this.table = newHT.table;
            this.chainsCount = newHT.chainsCount;
            this.capacity = newHT.capacity;
            this.size = newHT.size;
        }

        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            for (int i = 0; i < table.Length; i++)
            {
                output.Append(string.Format("[{0}] ->", i));
                if (table[i] != null)
                {
                    output.Append(string.Format("{0} [{1}]", table[i].value, Hash(table[i].key)));
                }

                output.Append("\n");
            }

            return output.ToString();
        }

        private int Hash(string key)
        {
            return Math.Abs(key.GetHashCode()) % capacity;
        }

        protected class Entry
        {
            public string key;
            public double value;

            public Entry() { }

            public Entry(string key, double value)
            {
                this.key = key;
                this.value = value;
            }
        }
    }
}
