using System;
using System.Collections.Generic;

using DistributedCipher.Framework;

namespace DistributedCipher.Common
{
    public class Scrambler : FerricNode, IScrambler
    {
        protected IList<int> scrambledIndexes;

        public IList<int> ScrambledIndexes { get { return this.scrambledIndexes; } }

        public Scrambler(Guid id, IList<int> scrambledIndexes)
            : this(id, scrambledIndexes, null)
        {
        }

        public Scrambler(Guid id, IList<int> scrambledIndexes, IFerricNode child)
            : base(id, child)
        {
            this.scrambledIndexes = scrambledIndexes;

            for (int i = 0; i < this.scrambledIndexes.Count; i++)
                if (Math.Abs(this.scrambledIndexes[i]) > i)
                    throw new InvalidOperationException("The index " + this.scrambledIndexes[i] + " is invalid because it cannot inserted in the " + i + "location.");
        }

        public override byte[] Decrypt(byte[] bytes)
        {
            IList<byte> decryptedBytes = new List<byte>();
            IList<byte> encryptedBytes = new List<byte>();

            if (child != null)
                bytes = this.child.Decrypt(bytes);

            foreach (byte bite in bytes)
                encryptedBytes.Add(bite);

            int scramblerIndex = bytes.Length % this.scrambledIndexes.Count;
            while(encryptedBytes.Count > 0)
            {
                scramblerIndex--;
                if (scramblerIndex < 0)
                    scramblerIndex = this.scrambledIndexes.Count - 1;

                int index = this.scrambledIndexes[scramblerIndex];

                if (index < 0)
                    index = encryptedBytes.Count - 1 + index;

                decryptedBytes.Insert(0, encryptedBytes[index]);
                encryptedBytes.RemoveAt(index);
            }

            byte[] decryptedByteArray = FerricHelper.ConvertToByteArray(decryptedBytes);

            return decryptedByteArray;
        }

        public override byte[] Encrypt(byte[] bytes)
        {
            IList<byte> encryptedBytes = new List<byte>();

            int scramblerIndex = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                int index = this.scrambledIndexes[scramblerIndex];

                if (index < 0)
                    index = encryptedBytes.Count + index;

                encryptedBytes.Insert(index, bytes[i]);

                scramblerIndex++;
                if (scramblerIndex == this.scrambledIndexes.Count)
                    scramblerIndex = 0;
            }

            byte[] encryptedByteArray = FerricHelper.ConvertToByteArray(encryptedBytes);

            if (child != null)
                encryptedByteArray = this.child.Encrypt(encryptedByteArray);

            return encryptedByteArray;
        }
    }
}
