using System;
using System.Collections.Generic;

using DistributedCipher.Framework;

namespace DistributedCipher.Common
{
    public class ByteIndex : IByteIndex
    {
        protected IList<byte> data;
        protected Guid id;

        public int Count { get { return Data.Count; } }
        
        public IList<byte> Data
        { 
            get { return this.data; }
            private set
            {
                this.data = value;

                for (int i = 0; i < this.data.Count; i++)
                {
                    byte biteToBeChecked = this.data[i];

                    for (int x = i + 1; x < this.data.Count; x++)
                        if (this.data[x] == biteToBeChecked)
                            throw new InvalidMappedKeyException("Duplicate Values found in Index: " + biteToBeChecked + "(" + Convert.ToInt32(biteToBeChecked) + ")");
                }
            }
        }

        public Guid ID { get { return this.id; } }

        public ByteIndex(IList<byte> bytes)
            : this(Guid.NewGuid(), bytes, false)
        {
        }

        public ByteIndex(Guid id, IList<byte> bytes)
            : this(id, bytes, false)
        {
        }

        public ByteIndex(Guid id, IList<byte> bytes, bool randomize)
        {
            this.id = id;

            IList<byte> temp;

            if (randomize)
            {
                IList<int> indexes = new List<int>();
                for (int i = 0; i < bytes.Count; i++)
                    indexes.Insert(FerricHelper.Random.Next(indexes.Count), i);

                temp = new List<byte>();
                foreach (int index in indexes)
                    temp.Add(bytes[index]);
            }
            else
            {
                temp = bytes;
            }

            Data = temp;
        }

        public byte Lookup(int index)
        {
            if (index < 0 || index >= this.data.Count)
                throw new IndexOutOfRangeException("Index: " + index);

            return this.data[index];
        }

        public int ReverseLookup(byte source)
        {
            int index = this.data.IndexOf(source);

            if (index == -1)
                throw new KeyNotFoundException("Source: " + source);

            return index;
        }
    }
}
