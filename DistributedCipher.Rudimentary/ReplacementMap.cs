using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using DistributedCipher.Framework;

namespace DistributedCipher.Rudimentary
{
    public class ReplacementMap : ICipherType
    {
        protected IByteMap byteMap;
        //protected Dictionary<byte, byte> byteMap;
        //protected Dictionary<byte, byte> reverseByteMap;

        public ReplacementMap(IByteMap byteMap)
        //public ReplacementMap(Dictionary<char, char> characterMap)
        {
            if (byteMap == null)
                throw new ArgumentNullException("The provided one way character map was Null.");

            this.byteMap = byteMap;

            //this.byteMap = new Dictionary<byte, byte>();
            //foreach (char key in characterMap.Keys)
            //    this.byteMap[Convert.ToByte(key)] = Convert.ToByte(characterMap[key]);

            //this.reverseByteMap = new Dictionary<byte, byte>();
            //foreach (byte key in this.byteMap.Keys)
            //{
            //    byte reverseKey = this.byteMap[key];
            //    if(this.reverseByteMap.ContainsKey(reverseKey))
            //    {
            //        StringBuilder stringBuilder = new StringBuilder();
            //        stringBuilder.Append("Duplicate values (");
            //        stringBuilder.Append(Convert.ToChar(reverseKey));
            //        stringBuilder.Append(") were mapped.");

            //        throw new InvalidMappedKeyException(stringBuilder.ToString());
            //    }

            //    this.reverseByteMap[reverseKey] = key;
            //}

            //foreach (byte reverseKey in this.reverseByteMap.Keys)
            //{
            //    if (!this.byteMap.ContainsKey(reverseKey))
            //    {
            //        StringBuilder stringBuilder = new StringBuilder();
            //        stringBuilder.Append("Key (");
            //        stringBuilder.Append(Convert.ToChar(reverseKey));
            //        stringBuilder.Append(") was not found in reverse lookup table.");

            //        throw new InvalidMappedKeyException(stringBuilder.ToString());
            //    }
            //}

            //foreach (byte key in this.byteMap.Keys)
            //{
            //    byte reverseKey = this.byteMap[key];

            //    if (key != this.reverseByteMap[reverseKey])
            //    {
            //        StringBuilder stringBuilder = new StringBuilder();
            //        stringBuilder.Append("Mapped Character: (");
            //        stringBuilder.Append(Convert.ToChar(key));
            //        stringBuilder.Append(", ");
            //        stringBuilder.Append(Convert.ToChar(reverseKey));
            //        stringBuilder.Append("); Reverse Mapped Character: (");
            //        stringBuilder.Append(Convert.ToChar(reverseKey));
            //        stringBuilder.Append(", ");
            //        stringBuilder.Append(Convert.ToChar(this.reverseByteMap[reverseKey]));
            //        stringBuilder.Append(");");

            //        throw new InvalidMappedKeyException(stringBuilder.ToString());
            //    }
            //}
        }

        //public ReplacementMap(IList<char> characterSet)
        //{
        //    if (characterSet == null)
        //        throw new ArgumentNullException("The provided set of characters to map was Null.");

        //    this.byteMap = new Dictionary<byte, byte>();
        //    this.reverseByteMap = new Dictionary<byte, byte>();

        //    List<char> temp = new List<char>();
        //    foreach (char character in characterSet)
        //        temp.Add(character);

        //    int randomIndex = DistributedCipherHelper.Random.Next(temp.Count);

        //    char firstCharacter = temp[randomIndex];
        //    temp.RemoveAt(randomIndex);

        //    char currentCharacter = firstCharacter;
        //    while(temp.Count > 0)
        //    {
        //        randomIndex = DistributedCipherHelper.Random.Next(temp.Count);

        //        this.byteMap[Convert.ToByte(currentCharacter)] = Convert.ToByte(temp[randomIndex]);
        //        currentCharacter = temp[randomIndex];
        //        temp.RemoveAt(randomIndex);
        //    }
        //    this.byteMap[Convert.ToByte(currentCharacter)] = Convert.ToByte(firstCharacter);

        //    foreach (byte key in this.byteMap.Keys)
        //        this.reverseByteMap[this.byteMap[key]] = key;
        //}

        public byte[] Decrypt(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException("The provided byte array was Null.");

            byte[] decryptedBytes = new byte[bytes.Length];

            for (int i = 0; i < bytes.Length; i++)
                decryptedBytes[i] = this.byteMap.ReverseLookup(bytes[i]);

            //for(int i = 0; i < bytes.Length; i++)
            //{
            //    if (this.reverseByteMap.ContainsKey(bytes[i]))
            //        decryptedBytes[i] = this.reverseByteMap[bytes[i]];
            //    else
            //        decryptedBytes[i] = bytes[i];
            //}

            return decryptedBytes;
        }

        public byte[] Encrypt(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException("The provided byte array was Null.");

            byte[] encryptedBytes = new byte[bytes.Length];

            for (int i = 0; i < bytes.Length; i++)
            {
                if (this.byteMap.ContainsKey(bytes[i]))
                    encryptedBytes[i] = this.byteMap[bytes[i]];
                else
                    encryptedBytes[i] = bytes[i];
            }

            return encryptedBytes;
        }
    }
}
