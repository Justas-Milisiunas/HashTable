using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HashTable
{
    class HashTableD : HashTableInt
    {
        private readonly double loadFactor = 0.75;
        private FileStream fs { get; set; }

        private string fileName;
        private int size;
        private int capacity;

        private int keySize = 8;
        private int chainsCount;

        public override int operationsCount { get; set; }


        public HashTableD(int capacity = 10, int keySize = 8,string fileName = "hashfile.dat")
        {
            this.capacity = capacity;
            this.size = 0;
            this.chainsCount = 0;
            this.fileName = fileName;
            this.keySize = keySize;
            this.operationsCount = 0;

            if (File.Exists(this.fileName))
                File.Delete(this.fileName);

            this.fs = new FileStream(this.fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            ResetTable();
        }

        public override double Put(string key, double value)
        {
            if (key == null)
            {
                throw new ArgumentException("Key or value is null in Put(string key, double value)");
            }

            bool keyFound;
            int foundIndex = FindPosition(key, out keyFound);
            if(foundIndex == -1)
            {
                Rehash(capacity * 2);
                Put(key, value);
            }
            else
            {
                Byte[] data = new Byte[8 + keySize];
                data = Encoding.UTF8.GetBytes(key);

                fs.Seek(foundIndex * (8 + keySize), SeekOrigin.Begin);
                fs.Write(data, 0, keySize);

                data = BitConverter.GetBytes(value);
                fs.Write(data, 0, 8);

                if(keyFound == false)
                    size++;

                if (size > capacity * loadFactor)
                    Rehash(capacity * 2);
            }

            return value;
        }

        public override double? Get(string key)
        {
            operationsCount++;
            if (key == null)
            {
                operationsCount++;
                throw new ArgumentNullException("Key is null in Get(string key)");
            }

            int index = Hash(key);
            int tempIndex = index;
            int j = 0;
            operationsCount += 4;

            for (int i = 0; i < capacity; i++)
            {
                Byte[] data = new Byte[8 + keySize];
                fs.Seek(index * (8 + keySize), SeekOrigin.Begin);
                fs.Read(data, 0, 8 + keySize);

                string tempKey = Encoding.UTF8.GetString(data, 0, keySize);
                double tempValue = BitConverter.ToDouble(data, keySize);

                operationsCount += 6;
                if (tempKey == "\0\0\0\0\0\0\0\0")
                {
                    operationsCount++;
                    return null;

                }
                operationsCount++;
                if (tempKey.Equals(key))
                {
                    operationsCount++;
                    return tempValue;
                }
                j++;
                index = (tempIndex + j) % capacity;
                operationsCount += 2;
            }

            operationsCount++;
            return null;
        }

        public override bool Contains(string key)
        {
            return Get(key) == null ? false : true;
        }

        private int FindPosition(string key, out bool keyFound)
        {
            if (key == null)
                throw new ArgumentNullException("Key is null");

            keyFound = false;
            int index = Hash(key);
            int tempIndex = index;
            int i = 0;

            for (int j = 0; j < capacity; j++)
            {
                Byte[] data = new Byte[8 + keySize];
                fs.Seek(index * (8 + keySize), SeekOrigin.Begin);
                fs.Read(data, 0, 8 + keySize);
                string tempKey = Encoding.UTF8.GetString(data, 0, 8);
                double tempValue = BitConverter.ToDouble(data, keySize);

                if (tempKey.Equals(key))
                {
                    keyFound = true;
                    return index;
                }

                if(tempKey == "\0\0\0\0\0\0\0\0")
                {
                    return index;
                }

                i++;
                index = (tempIndex + i) % capacity;
            }

            return -1;
        }

        private void Rehash(int capacity)
        {
            HashTableD newHT = new HashTableD(capacity, keySize, "rehashed" + System.DateTime.UtcNow.Ticks + ".dat");
            for(int i = 0; i < capacity; i++)
            {
                Byte[] data = new Byte[8 + keySize];
                fs.Seek(i * (8 + keySize), SeekOrigin.Begin);
                fs.Read(data, 0, 8 + keySize);
                string key = Encoding.UTF8.GetString(data, 0, keySize);
                double value = BitConverter.ToDouble(data, keySize);

                if(key != "\0\0\0\0\0\0\0\0")
                {
                    newHT.Put(key, value);
                }
            }
            this.fs.Close();
            File.Delete(this.fileName);

            this.fileName = newHT.fileName;
            this.fs = newHT.fs;
            this.chainsCount = newHT.chainsCount;
            this.capacity = newHT.capacity;
            this.size = newHT.size;
        }

        private void ResetTable()
        {
            for(int i = 0; i < capacity * (8 + keySize); i++)
            {
                //Byte[] data = new Byte[4];
                //data = BitConverter.GetBytes(-1);
                //fs.Seek(i * 4, SeekOrigin.Begin);
                //fs.Write(data, 0, 4);
                fs.WriteByte(0);
            }
            
        }

        private int Hash(string key)
        {
            return Math.Abs(key.GetHashCode()) % capacity;
        }

        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            for (int i = 0; i < capacity; i++)
            {
                Byte[] data = new Byte[8 + keySize];
                fs.Seek(i * (8 + keySize), SeekOrigin.Begin);
                fs.Read(data, 0, 8 + keySize);
                string key = Encoding.UTF8.GetString(data, 0, keySize);
                double value = BitConverter.ToDouble(data, keySize);

                output.Append(string.Format("[{0}] ->", i));

                if(key != "\0\0\0\0\0\0\0\0")
                    output.Append(string.Format("{0} {1} [{2}]", key, value, Hash(key)));
                output.Append("\n");
            }

            return output.ToString();
        }

        public string FileName => fileName;
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
