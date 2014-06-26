using System;
using System.Collections.Generic;

using DistributedCipher.Framework;

namespace DistributedCipher.Common
{
    public class RandomDataInserts : IRandomDataInserts
    {
        protected IByteIndex byteIndex;
        protected IDistributedCipher distributedCipher;

        public IByteIndex ByteIndex { get { return this.byteIndex; } }
        public ICipherType CipherType { get { throw new InvalidOperationException("Random Data Inserts don't have a Cipher Type.") ; } }
        public IDistributedCipher DistributedCipher { set { this.distributedCipher = value; /*this.distributedCipher.Register(this.byteIndex);*/ } }
        public int EncryptedLength { get { return 1; } }
        public int Length { get { return 0; } }

        public RandomDataInserts(IByteIndex byteIndex)
        {
            this.byteIndex = byteIndex;
        }

        public byte[] Decrypt(ref byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException();
            if (bytes.Length < 1)
                throw new ArgumentOutOfRangeException();

            byte[] newBytes = new byte[bytes.Length - 1];

            for (int i = 0; i < newBytes.Length; i++)
                newBytes[i] = bytes[i + 1];

            bytes = newBytes;

            return new byte[0];
        }

        public byte[] Encrypt(ref byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException();

            return new byte[1] { this.byteIndex.Lookup(FerricHelper.Random.Next(this.byteIndex.Count)) };
        }
    }
}
