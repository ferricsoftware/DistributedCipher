using System;
using System.Collections.Generic;

using DistributedCipher.Common;
using DistributedCipher.Framework;

namespace DistributedCipher.CaesarShift
{
    public enum ShiftDirection
    {
        Left,
        Right
    }

    public class CaesarShift : ICipherType
    {
        protected int amount;
        protected string name;
        protected ReplacementMap replacementMap;
        protected ShiftDirection shiftDirection;

        public int Amount { get { return this.amount; } }
        public string Name { get { return this.name; } set { this.name = value; } }
        public ShiftDirection ShiftDirection { get { return this.shiftDirection; } }

        public CaesarShift()
            : this(1, ShiftDirection.Right)
        {
        }

        public CaesarShift(int amount)
            : this(amount, ShiftDirection.Right)
        {
        }

        public CaesarShift(int amount, ShiftDirection shiftDirection)
        {
            this.amount = amount;
            this.shiftDirection = shiftDirection;

            Dictionary<byte, byte> letterMap = new Dictionary<byte, byte>();

            int mappedIndex;
            if (this.shiftDirection == ShiftDirection.Left)
                mappedIndex = -this.amount;
            else
                mappedIndex = this.amount;

            for (int i = 0; i < FerricHelper.LowerCaseLetters.Count; i++)
            {
                if (mappedIndex < 0)
                    mappedIndex += FerricHelper.LowerCaseLetters.Count;
                else if (mappedIndex >= FerricHelper.LowerCaseLetters.Count)
                    mappedIndex -= FerricHelper.LowerCaseLetters.Count;

                letterMap[Convert.ToByte(FerricHelper.LowerCaseLetters[i])] = Convert.ToByte(FerricHelper.LowerCaseLetters[mappedIndex]);
                letterMap[Convert.ToByte(FerricHelper.UpperCaseLetters[i])] = Convert.ToByte(FerricHelper.UpperCaseLetters[mappedIndex]);

                mappedIndex++;
            }

            this.replacementMap = new ReplacementMap(new ByteMap(letterMap));
        }

        public byte[] Decrypt(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException("Bytes");

            return this.replacementMap.Decrypt(bytes);
        }

        public byte[] Encrypt(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException("Bytes");

            return this.replacementMap.Encrypt(bytes);
        }
    }
}
