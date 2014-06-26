using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedCipher.Framework
{
    public static class FerricHelper
    {
        public static Random Random = new Random();

        internal static IList<byte> lowerCaseLetters;
        public static IList<byte> LowerCaseLetters
        {
            get
            {
                if (lowerCaseLetters == null)
                    lowerCaseLetters = FerricHelper.ConvertToByteList(
                        new List<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' });

                return lowerCaseLetters;
            }
        }

        internal static IList<byte> upperCaseLetters;
        public static IList<byte> UpperCaseLetters
        {
            get
            {
                if (upperCaseLetters == null)
                    upperCaseLetters = FerricHelper.ConvertToByteList(
                        new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' });

                return upperCaseLetters;
            }
        }

        internal static IList<byte> letters;
        public static IList<byte> Letters
        {
            get
            {
                if (letters == null)
                {
                    letters = new List<byte>();

                    foreach (byte letter in FerricHelper.LowerCaseLetters)
                        letters.Add(letter);

                    foreach (byte letter in FerricHelper.UpperCaseLetters)
                        letters.Add(letter);
                }

                return letters;
            }
        }

        internal static IList<byte> numbers;
        public static IList<byte> Numbers
        {
            get
            {
                if (numbers == null)
                    numbers = FerricHelper.ConvertToByteList(
                        new List<char>() { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' });

                return numbers;
            }
        }

        internal static IList<byte> alphaNumeric;
        public static IList<byte> AlphaNumeric
        {
            get
            {
                if (alphaNumeric == null)
                {
                    alphaNumeric = new List<byte>();

                    foreach (byte letter in FerricHelper.Letters)
                        alphaNumeric.Add(letter);

                    foreach (byte number in FerricHelper.Numbers)
                        alphaNumeric.Add(number);
                }

                return alphaNumeric;
            }
        }

        internal static IList<byte> keyboardCharacters;
        public static IList<byte> KeyboardCharacters
        {
            get
            {
                if (keyboardCharacters == null)
                {
                    keyboardCharacters = FerricHelper.ConvertToByteList(new List<char>() { 
                        '`', '-', '=', '\b', '~', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '_', '+',
                        '\t', '[', ']', '\\', '{', '}', '|',
                        ';', '\'', ':', '"', '\r', '\n',
                        ',', '.', '/', '<', '>', '?',
                        ' '
                    });

                    foreach (byte alphaNumericCharacter in FerricHelper.AlphaNumeric)
                        keyboardCharacters.Add(alphaNumericCharacter);
                }

                return keyboardCharacters;
            }
        }

        public static byte[] ConvertToByteArray(IList<byte> bytes)
        {
            byte[] convertedBytes = new byte[bytes.Count];

            for (int i = 0; i < bytes.Count; i++)
                convertedBytes[i] = bytes[i];

            return convertedBytes;
        }

        public static byte[] ConvertToByteArray(IList<char> characters)
        {
            byte[] convertedBytes = new byte[characters.Count];

            for (int i = 0; i < characters.Count; i++)
                convertedBytes[i] = Convert.ToByte(characters[i]);

            return convertedBytes;
        }

        public static byte[] ConvertToByteArray(string message)
        {
            return FerricHelper.ConvertToByteArray(message.ToCharArray());
        }

        public static IList<byte> ConvertToByteList(IList<byte> bytes)
        {
            return new List<byte>(bytes);
        }

        public static IList<byte> ConvertToByteList(IList<char> characters)
        {
            IList<byte> bytes = new List<byte>();

            foreach (char character in characters)
                bytes.Add(Convert.ToByte(character));

            return bytes;
        }

        public static IList<char> ConvertToCharacterList(byte[] bytes)
        {
            IList<char> characters = new List<char>();

            foreach (byte bite in bytes)
                characters.Add(Convert.ToChar(bite));

            return characters;
        }

        public static IList<char> ConvertToCharacterList(string message)
        {
            IList<char> characters = new List<char>();

            foreach (char character in message.ToCharArray())
                characters.Add(character);

            return characters;
        }

        public static string ConvertToString(byte[] bytes)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (byte bite in bytes)
                stringBuilder.Append(Convert.ToChar(bite));

            return stringBuilder.ToString();
        }

        public static string ConvertToString(IList<byte> bytes)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (byte bite in bytes)
                stringBuilder.Append(Convert.ToChar(bite));

            return stringBuilder.ToString();
        }

        public static string ConvertToString(IList<char> characters)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char character in characters)
                stringBuilder.Append(character);

            return stringBuilder.ToString();
        }

        public static string Crush(IList<byte> data)
        {
            string crushedBytes = string.Empty;

            foreach (byte bite in data)
                crushedBytes += Convert.ToInt32(bite).ToString() + ",";

            return crushedBytes.Substring(0, crushedBytes.Length - 1);
        }

        public static string Crush(Dictionary<byte, byte> data)
        {
            string crushedBytes = string.Empty;

            int i = 0;
            foreach (byte key in data.Keys)
            {
                crushedBytes += Convert.ToInt32(key).ToString();
                crushedBytes += ",";
                crushedBytes += Convert.ToInt32(data[key]).ToString();
                crushedBytes += ",";

                i++;
            }

            return crushedBytes.Substring(0, crushedBytes.Length - 1);
        }

        public static byte[] Flatten(IList<byte> data)
        {
            return FerricHelper.ConvertToByteArray(data);
        }

        public static byte[] Flatten(Dictionary<byte, byte> data)
        {
            byte[] flattenedBytes = new byte[data.Count * 2];

            int i = 0;
            foreach (byte key in data.Keys)
            {
                flattenedBytes[(i * 2) + 0] = key;
                flattenedBytes[(i * 2) + 1] = data[key];

                i++;
            }

            return flattenedBytes;
        }

        public static Dictionary<byte, byte> UncrushDictionary(string data)
        {
            Dictionary<byte, byte> dictionary = new Dictionary<byte, byte>();

            string[] splitData = data.Split(new char[] { ',' });

            for (int i = 0; i < splitData.Length / 2; i++)
            {
                byte key = Convert.ToByte(Convert.ToInt32(splitData[i * 2 + 0]));
                byte _value = Convert.ToByte(Convert.ToInt32(splitData[i * 2 + 1]));

                dictionary[key] = _value;
            }

            return dictionary;
        }

        public static byte[] UncrushArray(string data)
        {
            string[] splitData = data.Split(new char[] { ',' });
            byte[] array = new byte[splitData.Length];

            for (int i = 0; i < splitData.Length; i++)
                array[i] = Convert.ToByte(Convert.ToInt32(splitData[i]));

            return array;
        }

        public static IList<byte> UnflattenArray(byte[] data)
        {
            return FerricHelper.ConvertToByteList(data);
        }

        public static Dictionary<byte, byte> UnflattenDictionary(byte[] data)
        {
            return null;
        }
    }
}
