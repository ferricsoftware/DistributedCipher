using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using DistributedCipher.Framework;

namespace DistributedCipher.Test.Unit
{
    public abstract class IScramblerTest
    {
        protected IList<int> scrambledIndexes;
        protected IScrambler scramblerUnderTest;

        [TestInitialize()]
        public virtual void TestInitialize()
        {
        }

        [TestCleanup()]
        public virtual void TestCleanup()
        {
            this.scrambledIndexes = null;
            this.scramblerUnderTest = null;
        }

        [TestMethod]
        public void Decrypt_Encrypted_Array_Test_Arrays_Match()
        {
            //Arrange
            byte[] actual;
            byte[] temp;
            byte[] expected = FerricHelper.ConvertToByteArray(new List<char> {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j'
            });

            //Act
            temp = this.scramblerUnderTest.Encrypt(expected);

            actual = this.scramblerUnderTest.Decrypt(temp);

            //Assert
            Assert.AreEqual(expected.Length, actual.Length);

            for (int i = 0; i < actual.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void Decrypt_Encrypted_String_Test_Arrays_Match()
        {
            //Arrange
            string actual;
            string expected = "ABCDEFGHIJ";

            //Act
            actual = this.scramblerUnderTest.Decrypt(this.scramblerUnderTest.Encrypt(expected));

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Decrypt_Test_Returns_Decrypted_Byte_Array()
        {
            //Arrange
            byte[] actual;
            byte[] expected = FerricHelper.ConvertToByteArray(new List<char> {
                'a', 'b', 'c'
            });

            //Act
            actual = this.scramblerUnderTest.Decrypt(expected);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Length, actual.Length);
        }

        [TestMethod]
        public void Decrypt_Test_Returns_Decrypted_String()
        {
            //Arrange
            string actual;
            string expected = "std";

            //Act
            actual = this.scramblerUnderTest.Decrypt(expected);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Length, actual.Length);
        }

        [TestMethod]
        public void Encrypt_Test_Returns_Encrypted_Byte_Array()
        {
            //Arrange
            byte[] actual;
            byte[] expected = FerricHelper.ConvertToByteArray(new List<char> {
                'a', 'n', 'b'
            });

            //Act
            actual = this.scramblerUnderTest.Encrypt(expected);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Length, actual.Length);
        }

        [TestMethod]
        public void Encrypt_Test_Returns_Encrypted_String()
        {
            //Arrange
            string actual;
            string expected = "xas";

            //Act
            actual = this.scramblerUnderTest.Encrypt(expected);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Length, actual.Length);
        }
    }
}
