using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using DistributedCipher.CaesarShift;
using DistributedCipher.Common;
using DistributedCipher.Framework;

namespace DistributedCipher.Test.Unit.Extensions
{
    [TestClass]
    public class CaesarShiftTest : ICipherTypeTest
    {
        [TestInitialize()]
        public void TestInitialize()
        {
            this.cipherTypeUnderTest = new CaesarShift.CaesarShift(1, ShiftDirection.Left);
        }

        [TestMethod]
        public void Decrypt_1_Left_Test_Properly_Decodes_Text()
        {
            //Arrange
            byte[] actual;
            byte[] expected = FerricHelper.ConvertToByteArray(new List<char>() { 'y', 'z', 'a', 'b', 'c', 'Y', 'Z', 'A', 'B', 'C' });
            byte[] injected = FerricHelper.ConvertToByteArray(new List<char>() { 'x', 'y', 'z', 'a', 'b', 'X', 'Y', 'Z', 'A', 'B' });

            this.cipherTypeUnderTest = new CaesarShift.CaesarShift(1, ShiftDirection.Left);

            //Act
            actual = this.cipherTypeUnderTest.Decrypt(injected);

            //Assert
            for (int i = 0; i < actual.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void Decrypt_1_Right_Test_Properly_Decodes_Text()
        {
            //Arrange
            byte[] actual;
            byte[] expected = FerricHelper.ConvertToByteArray(new List<char>() { 'x', 'y', 'z', 'a', 'b', 'X', 'Y', 'Z', 'A', 'B' });
            byte[] injected = FerricHelper.ConvertToByteArray(new List<char>() { 'y', 'z', 'a', 'b', 'c', 'Y', 'Z', 'A', 'B', 'C' });

            this.cipherTypeUnderTest = new CaesarShift.CaesarShift(1, ShiftDirection.Right);

            //Act
            actual = this.cipherTypeUnderTest.Decrypt(injected);

            //Assert
            for (int i = 0; i < actual.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void Decrypt_3_Right_Test_Properly_Decodes_Text()
        {
            //Arrange
            byte[] actual;
            byte[] injected = FerricHelper.ConvertToByteArray(new List<char>() { 'y', 'z', 'a', 'b', 'c', 'd', 'e', 'Y', 'Z', 'A', 'B', 'C', 'D', 'E' });
            byte[] expected = FerricHelper.ConvertToByteArray(new List<char>() { 'v', 'w', 'x', 'y', 'z', 'a', 'b', 'V', 'W', 'X', 'Y', 'Z', 'A', 'B' });

            this.cipherTypeUnderTest = new CaesarShift.CaesarShift(3, ShiftDirection.Right);

            //Act
            actual = this.cipherTypeUnderTest.Decrypt(injected);

            //Assert
            for (int i = 0; i < actual.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void Encrypt_1_Left_Test_Properly_Encodes_Text()
        {
            //Arrange
            byte[] actual;
            byte[] expected = FerricHelper.ConvertToByteArray(new List<char>() { 'x', 'y', 'z', 'a', 'b', 'X', 'Y', 'Z', 'A', 'B' });
            byte[] injected = FerricHelper.ConvertToByteArray(new List<char>() { 'y', 'z', 'a', 'b', 'c', 'Y', 'Z', 'A', 'B', 'C' });

            this.cipherTypeUnderTest = new CaesarShift.CaesarShift(1, ShiftDirection.Left);

            //Act
            actual = this.cipherTypeUnderTest.Encrypt(injected);

            //Assert
            for (int i = 0; i < actual.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void Encrypt_1_Right_Test_Properly_Encodes_Text()
        {
            //Arrange
            byte[] actual;
            byte[] expected = FerricHelper.ConvertToByteArray(new List<char>() { 'y', 'z', 'a', 'b', 'c', 'Y', 'Z', 'A', 'B', 'C' });
            byte[] injected = FerricHelper.ConvertToByteArray(new List<char>() { 'x', 'y', 'z', 'a', 'b', 'X', 'Y', 'Z', 'A', 'B' });

            this.cipherTypeUnderTest = new CaesarShift.CaesarShift(1, ShiftDirection.Right);

            //Act
            actual = this.cipherTypeUnderTest.Encrypt(injected);

            //Assert
            for (int i = 0; i < actual.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void Encrypt_3_Right_Test_Properly_Encodes_Text()
        {
            //Arrange
            byte[] actual;
            byte[] injected = FerricHelper.ConvertToByteArray(new List<char>() { 'v', 'w', 'x', 'y', 'z', 'a', 'b', 'V', 'W', 'X', 'Y', 'Z', 'A', 'B' });
            byte[] expected = FerricHelper.ConvertToByteArray(new List<char>() { 'y', 'z', 'a', 'b', 'c', 'd', 'e', 'Y', 'Z', 'A', 'B', 'C', 'D', 'E' });

            this.cipherTypeUnderTest = new CaesarShift.CaesarShift(3, ShiftDirection.Right);

            //Act
            actual = this.cipherTypeUnderTest.Encrypt(injected);

            //Assert
            for (int i = 0; i < actual.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }
    }
}
