using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using DistributedCipher.Framework;

namespace DistributedCipher.Common
{
    public class ReplacementMap : ICipherType
    {
        protected IByteMap byteMap;
        protected string name;

        public IByteMap ByteMap { get { return byteMap; } }
        public string Name { get { return name; } set { this.name = value; } }
        
        public ReplacementMap(IByteMap byteMap)
        {
            if (byteMap == null)
                throw new ArgumentNullException("The provided one way character map was Null.");

            this.byteMap = byteMap;
        }

        public byte[] Decrypt(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException("The provided byte array was Null.");

            byte[] decryptedBytes = new byte[bytes.Length];

            for (int i = 0; i < bytes.Length; i++)
                decryptedBytes[i] = this.byteMap.ReverseLookup(bytes[i]);

            return decryptedBytes;
        }

        public byte[] Encrypt(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException("The provided byte array was Null.");

            byte[] encryptedBytes = new byte[bytes.Length];

            for (int i = 0; i < bytes.Length; i++)
                encryptedBytes[i] = this.byteMap.Lookup(bytes[i]);

            return encryptedBytes;
        }
    }
}
