using System;
using System.Collections.Generic;

using DistributedCipher.Framework;

namespace DistributedCipher.Rudimentary
{
    public enum ShiftDirection
    {
        Left,
        Right
    }

    public class CaesarShift : ICipherType
    {
        protected int amount;
        protected ReplacementMap lowerCaseReplacementMap;
        protected ShiftDirection shiftDirection;
        protected ReplacementMap upperCaseReplacementMap;

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

            Dictionary<char, char> lowerCaseLetterMap = new Dictionary<char, char>();
            Dictionary<char, char> upperCaseLetterMap = new Dictionary<char, char>();

            int mappedIndex;
            if (this.shiftDirection == ShiftDirection.Left)
                mappedIndex = -this.amount;
            else
                mappedIndex = this.amount;

            for (int i = 0; i < DistributedCipherHelper.LowerCaseLetters.Count; i++)
            {
                if (mappedIndex < 0)
                    mappedIndex += DistributedCipherHelper.LowerCaseLetters.Count;
                else if (mappedIndex >= DistributedCipherHelper.LowerCaseLetters.Count)
                    mappedIndex -= DistributedCipherHelper.LowerCaseLetters.Count;

                lowerCaseLetterMap[DistributedCipherHelper.LowerCaseLetters[i]] = DistributedCipherHelper.LowerCaseLetters[mappedIndex];
                upperCaseLetterMap[DistributedCipherHelper.UpperCaseLetters[i]] = DistributedCipherHelper.UpperCaseLetters[mappedIndex];

                mappedIndex++;
            }

            this.lowerCaseReplacementMap = new ReplacementMap(new ByteMap(lowerCaseLetterMap));
            this.upperCaseReplacementMap = new ReplacementMap(upperCaseLetterMap);
        }

        public byte[] Decrypt(byte[] bytes)
        {
            return this.lowerCaseReplacementMap.Decrypt(this.upperCaseReplacementMap.Decrypt(bytes));
        }

        public byte[] Encrypt(byte[] bytes)
        {
            return this.lowerCaseReplacementMap.Encrypt(this.upperCaseReplacementMap.Encrypt(bytes));
        }
    }
}
