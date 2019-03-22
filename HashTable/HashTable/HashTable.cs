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
        private static readonly double loadFactor = 0.75;

        private Node[] table;
        private int size;
        private int capacity;
        private int chainsCount;

        public HashTable(int capacity = 10)
        {
            this.capacity = capacity;
            this.table = new Node[capacity];
        }

        public Boolean Contains(string key)
        {
            return Get(key) != null;
        }

        public double Put(string key, double value)
        {
            if(key == null || value == null)
            {
                throw new ArgumentException("Key or value is null in Put(string key, double value)");
            }

            int index = Hash(key);
            if(table[index] == null)
            {
                chainsCount++;
            }

            Node node = GetInChain(key, table[index]);
            if(node == null)
            {
                Node tmp = new Node(key, value, table[index]);
                table[index] = tmp;
                size++;

                if(size > this.capacity * loadFactor)
                    Rehash(this.capacity * 2);
            }
            else
                node.value = value;

            return value;
        }

        public double Get(string key)
        {
            if (key == null)
                throw new ArgumentNullException("Key is null in Get(string key)");

            int index = Hash(key);
            Node node = GetInChain(key, table[index]);

            return (node != null) ? node.value : -1;
        }

        private Node GetInChain(string key, Node node)
        {
            if (key == null)
                throw new ArgumentNullException("Key is null");

            int chainSize = 0;
            for(Node i = node; i != null; i = i.next)
            {
                chainSize++;
                if ((i.key).Equals(key))
                    return i;
            }

            return null;
        }

        private void Rehash(int capacity)
        {
            HashTable newHT = new HashTable(capacity);
            for(int i = 0; i < table.Length; i++)
            {
                while(table[i] != null)
                {
                    newHT.Put(table[i].key, table[i].value);
                    table[i] = table[i].next;
                }
            }

            this.table = newHT.table;
            this.chainsCount = newHT.chainsCount;
            this.capacity = newHT.capacity;
            this.size = newHT.size;
        }

        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            for(int i = 0; i < table.Length; i++)
            {
                while(table[i] != null)
                {
                    output.Append(table[i].value + " ");
                    table[i] = table[i].next;
                }
                output.AppendLine();
            }

            return output.ToString();
        }

        private int Hash(string key)
        {
            return Math.Abs(key.GetHashCode()) % capacity;
        }

        private class Node
        {
            public string key;
            public double value;
            public Node next;

            public Node() { }

            public Node(string key, double value, Node next)
            {
                this.key = key;
                this.value = value;
                this.next = next;
            }
        }
    }
}
