using System;
using System.Collections.Generic;

using DistributedCipher.Framework;

namespace DistributedCipher.Common
{
    public class DistributedCipher : FerricNode, IDistributedCipher
    {
        protected IByteSetRepository byteSetRepository;
        protected IList<ICipher> ciphers;
        protected IHeader header;
        protected Random random;

        public IByteSetRepository ByteSetRepository { get { return this.byteSetRepository; } }
        public IList<ICipher> Ciphers { get { return this.ciphers; } }
        public IHeader Header { get { return this.header; } }

        public DistributedCipher(Guid id, IByteSetRepository byteSetRepository, IHeader header, IList<ICipher> ciphers, Random random)
            : this(id, byteSetRepository, header, ciphers, null, random)
        {
        }

        public DistributedCipher(Guid id, IByteSetRepository byteSetRepository, IHeader header, IList<ICipher> ciphers, IFerricNode child, Random random)
            : base(id, child)
        {
            this.byteSetRepository = byteSetRepository;
            this.ciphers = ciphers;
            this.header = header;
            this.id = id;
            this.random = random;

            if (this.header != null)
                this.header.DistributedCipher = this;
        }

        public override byte[] Decrypt(byte[] bytes)
        {
            int index;
            int offset;

            if (child != null)
                bytes = this.child.Decrypt(bytes);

            int startingOffset;
            if (this.header == null)
                startingOffset = 0;
            else
                startingOffset = this.header.Decrypt(ref bytes);

            int decryptedLength = 0;
            int remainingEncryptedLength = bytes.Length;

            offset = startingOffset;
            while (remainingEncryptedLength > 0)
            {
                decryptedLength += this.ciphers[offset].Length;
                remainingEncryptedLength -= this.ciphers[offset].EncryptedLength;

                offset++;
                if (offset >= this.ciphers.Count)
                    offset = 0;
            }

            byte[] decryptedBytes = new byte[decryptedLength];

            index = 0;
            offset = startingOffset;
            while (index < decryptedBytes.Length)
            {
                ICipher cipher = this.ciphers[offset];

                offset++;
                if (offset >= this.ciphers.Count)
                    offset = 0;

                int priorLength = bytes.Length;
                byte[] temp = cipher.Decrypt(ref bytes);

                if (priorLength != bytes.Length + cipher.EncryptedLength)
                    throw new ArgumentException("The correct number of claimed bytes were not removed. Expected: " + cipher.EncryptedLength + ", Actual: " + (priorLength - bytes.Length));

                if (temp.Length != cipher.Length)
                    throw new ArgumentException("The decrypted information does not match the claimed length. Expected: " + cipher.Length + ", Actual: " + temp.Length);

                for (int i = 0; i < temp.Length; i++)
                    decryptedBytes[index++] = temp[i];
            }

            return decryptedBytes;
        }

        public override byte[] Encrypt(byte[] bytes)
        {
            int index;
            int offset;

            int startingOffset;
            if (this.header == null)
                startingOffset = 0;
            else
                startingOffset = this.random.Next(this.ciphers.Count);

            int encryptedLength = 0;
            int remainingDecryptedLength = bytes.Length;

            offset = startingOffset;
            while (remainingDecryptedLength > 0)
            {
                encryptedLength += this.ciphers[offset].EncryptedLength;
                remainingDecryptedLength -= this.ciphers[offset].Length;

                offset++;
                if (offset >= this.ciphers.Count)
                    offset = 0;
            }

            int requiredByteLength = encryptedLength;

            if (this.header != null)
                requiredByteLength += this.header.EncryptedLength;

            byte[] encryptedBytes = new byte[requiredByteLength];

            byte[] headerBytes;
            if (this.header == null)
                headerBytes = new byte[0];
            else
                headerBytes = this.header.Encrypt(startingOffset);

            for (index = 0; index < headerBytes.Length; index++)
                encryptedBytes[index] = headerBytes[index];

            offset = startingOffset;
            while (index < encryptedBytes.Length)
            {
                ICipher cipher = this.ciphers[offset];

                offset++;
                if (offset >= this.ciphers.Count)
                    offset = 0;

                int priorLength = bytes.Length;
                byte[] temp = cipher.Encrypt(ref bytes);

                if (priorLength != bytes.Length + cipher.Length)
                    throw new ArgumentException("The correct number of claimed bytes were not removed. Expected: " + cipher.Length + ", Actual: " + (priorLength - bytes.Length));

                if (temp.Length != cipher.EncryptedLength)
                    throw new ArgumentException("The encrypted information does not match the claimed length. Expected: " + cipher.EncryptedLength + ", Actual: " + temp.Length);

                for (int i = 0; i < temp.Length; i++)
                    encryptedBytes[index++] = temp[i];
            }

            if (child != null)
                encryptedBytes = this.child.Encrypt(encryptedBytes);

            return encryptedBytes;
        }
    }
}
