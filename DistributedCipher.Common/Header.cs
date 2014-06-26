using System;
using System.Collections.Generic;
using System.Text;

using DistributedCipher.Framework;

namespace DistributedCipher.Common
{
    public class Header : IHeader
    {
        protected IList<bool> assignments;
        protected IByteIndex byteIndex;
        protected IDistributedCipher distributedCipher;
        protected int preferredLength;

        public IDistributedCipher DistributedCipher
        {
            set 
            { 
                this.distributedCipher = value; 
                //this.distributedCipher.Register(this.byteIndex);

                if (this.assignments == null)
                {
                    int minimumLength = Convert.ToInt32(this.distributedCipher.Ciphers.Count / (this.byteIndex.Count - 1));
                    if (this.distributedCipher.Ciphers.Count % (this.byteIndex.Count - 1) > 0)
                        minimumLength++;

                    IList<bool> temp = new List<bool>();
                    for (int i = 0; i < minimumLength; i++)
                        temp.Add(true);
                    for (int i = temp.Count; i < this.preferredLength; i++)
                        temp.Add(false);

                    this.assignments = new List<bool>();
                    while (temp.Count > 0)
                    {
                        int index = FerricHelper.Random.Next(temp.Count);

                        this.assignments.Add(temp[index]);

                        temp.RemoveAt(index);
                    }
                }
            }
        }
        
        public string Key
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();

                foreach (bool assignment in this.assignments)
                {
                    if (assignment)
                        stringBuilder.Append('1');
                    else
                        stringBuilder.Append('0');
                }

                return stringBuilder.ToString(); 
            }
        }

        public IByteIndex ByteIndex { get { return this.byteIndex; } }
        public int EncryptedLength { get { if (this.assignments == null) throw new ArgumentNullException("Assignments"); return this.assignments.Count; } }

        public Header(IByteIndex byteIndex, int preferredLength)
        {
            this.assignments = null;
            this.byteIndex = byteIndex;
            this.preferredLength = preferredLength;
        }

        public Header(IByteIndex byteIndex, string key)
        {
            this.byteIndex = byteIndex;

            this.assignments = new List<bool>();
            foreach (char character in key)
            {
                if (character == '0')
                    this.assignments.Add(false);
                else
                    this.assignments.Add(true);
            }
        }

        public int Decrypt(ref byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException("Bytes");

            byte[] header = new byte[this.assignments.Count];
            for (int i = 0; i < this.assignments.Count; i++)
                header[i] = bytes[i];

            byte[] resultingBytes = new byte[bytes.Length - this.assignments.Count];
            for (int i = 0; i < resultingBytes.Length; i++)
                resultingBytes[i] = bytes[i + this.assignments.Count];
            bytes = resultingBytes;

            int indexOffset = 0;
            for (int i = 0; i < this.assignments.Count; i++)
                if (this.assignments[i])
                    indexOffset += this.byteIndex.ReverseLookup(header[i]);

            return indexOffset;
        }

        public byte[] Encrypt(int indexOffset)
        {
            if (indexOffset >= this.distributedCipher.Ciphers.Count)
                throw new ArgumentException("Index Offset must not equal or be greater than the total number of Ciphers to cycle through.");

            IList<int> distribution = new List<int>();
            foreach (bool assignment in assignments)
                if (assignment)
                    distribution.Add(0);

            IList<int> finalDistribution = new List<int>();
            for (int i = 0; i < indexOffset; i++)
            {
                int tempIndex = FerricHelper.Random.Next(distribution.Count);

                distribution[tempIndex]++;

                if (distribution[tempIndex] == this.byteIndex.Count - 1)
                {
                    finalDistribution.Add(distribution[tempIndex]);

                    distribution.RemoveAt(tempIndex);
                }
            }

            foreach (int offsetPiece in distribution)
                finalDistribution.Add(offsetPiece);

            byte[] bytes = new byte[this.assignments.Count];

            for (int i = 0; i < this.assignments.Count; i++)
            {
                if (this.assignments[i])
                {
                    int randomIndex = FerricHelper.Random.Next(finalDistribution.Count);

                    int index = finalDistribution[randomIndex];
                    finalDistribution.RemoveAt(randomIndex);

                    bytes[i] = this.byteIndex.Lookup(index);
                }
                else
                {
                    bytes[i] = this.byteIndex.Lookup(FerricHelper.Random.Next(this.byteIndex.Count));
                }
            }

            return bytes;
        }
    }
}
