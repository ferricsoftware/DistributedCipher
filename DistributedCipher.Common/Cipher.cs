using System;
using System.Collections.Generic;

using DistributedCipher.Framework;

namespace DistributedCipher.Common
{
    public class Cipher : ICipher
    {
        protected int encryptedLength;
        protected ICipherType cipherType;
        protected IDistributedCipher distributedCipher;
        protected string key;
        protected int length;

        public ICipherType CipherType { get { return this.cipherType; } }
        public int EncryptedLength
        {
            get
            {
                if (this.encryptedLength == 0)
                {
                    byte[] bytes = new byte[this.length];
                    for (int i = 0; i < this.length; i++)
                        bytes[i] = 1;

                    this.encryptedLength = this.cipherType.Encrypt(bytes).Length;
                }

                return this.encryptedLength;
            }
        }

        public string Key { get { return this.key; } }
        public int Length { get { return this.length; } }

        public Cipher(ICipherType cipherType) : this(cipherType, 1)
        {
        }

        public Cipher(ICipherType cipherType, int length)
        {
            this.cipherType = cipherType;
            this.length = length;

            this.encryptedLength = 0;
        }

        public byte[] Decrypt(ref byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException();
            if (bytes.Length < this.encryptedLength)
                throw new ArgumentOutOfRangeException();

            byte[] bytesToDecrypt = new byte[this.encryptedLength];
            for (int i = 0; i < bytesToDecrypt.Length; i++)
                bytesToDecrypt[i] = bytes[i];

            byte[] newBytes = new byte[bytes.Length - this.encryptedLength];
            for (int i = 0; i < newBytes.Length; i++)
                newBytes[i] = bytes[this.encryptedLength + i];

            bytes = newBytes;

            return this.cipherType.Decrypt(bytesToDecrypt);
        }

        public byte[] Encrypt(ref byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException();
            if (bytes.Length < this.length)
                throw new ArgumentOutOfRangeException();

            byte[] bytesToEncrypt = new byte[this.length];
            for (int i = 0; i < bytesToEncrypt.Length; i++)
                bytesToEncrypt[i] = bytes[i];

            byte[] newBytes = new byte[bytes.Length - this.length];
            for (int i = 0; i < newBytes.Length; i++)
                newBytes[i] = bytes[i + this.length];

            bytes = newBytes;

            return this.cipherType.Encrypt(bytesToEncrypt);
        }
    }
}
