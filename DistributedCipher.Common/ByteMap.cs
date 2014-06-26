using System;
using System.Collections.Generic;

using DistributedCipher.Framework;

namespace DistributedCipher.Common
{
    public class ByteMap : IByteMap
    {
        protected Dictionary<byte, byte> data;
        protected Guid id;
        protected Dictionary<byte, byte> reverseData;

        public int Count { get { return Data.Count; } }

        public Dictionary<byte, byte> Data
        {
            get { return this.data; }
            private set
            {
                this.data = value;

                this.reverseData = new Dictionary<byte, byte>();
                foreach (byte key in this.data.Keys)
                    this.reverseData[this.data[key]] = key;

                foreach (byte key in this.data.Keys)
                    if (!this.reverseData.ContainsKey(key))
                        throw new InvalidMappedKeyException("Lookup: " + key + " (" + key + ")");

                foreach (byte key in this.reverseData.Keys)
                    if (!this.data.ContainsKey(key))
                        throw new InvalidMappedKeyException("Reverse Lookup: " + key + " (" + key + ")");
            }
        }

        public Guid ID { get { return this.id; } }

        public ByteMap(Dictionary<byte, byte> data)
            : this(Guid.NewGuid(), data)
        {
        }

        public ByteMap(Guid id, Dictionary<byte, byte> data)
        {
            if (data == null)
                throw new ArgumentNullException("Data");

            this.id = id;

            Data = data;
        }

        public ByteMap(IList<byte> data)
            : this(Guid.NewGuid(), data)
        {
        }

        public ByteMap(Guid id, IList<byte> data)
        {
            if (data == null)
                throw new ArgumentNullException("Data");

            this.id = id;

            IList<int> indexes = new List<int>();
            for (int i = 0; i < data.Count; i++)
                indexes.Insert(FerricHelper.Random.Next(indexes.Count), i);

            Dictionary<byte, byte> randomizedData = new Dictionary<byte, byte>();
            for (int i = 0; i < indexes.Count; i++)
                randomizedData[data[i]] = data[indexes[i]];

            Data = randomizedData;
        }

        public byte Lookup(byte source)
        {
            if (this.data.ContainsKey(source))
                return this.data[source];

            return source;
        }

        public byte ReverseLookup(byte source)
        {
            if (this.data.ContainsKey(source))
                return this.reverseData[source];

            return source;
        }
    }
}
