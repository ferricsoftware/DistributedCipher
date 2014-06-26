using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

using DistributedCipher.Framework;

namespace DistributedCipher.Test.Unit
{
    public abstract class IRandomDataInsertsTest
    {
        protected IRandomDataInserts randomDataUnderTest;
        
        private TestContext testContextInstance;
        public TestContext TestContext { get { return testContextInstance; } set { testContextInstance = value; } }

        [TestCleanup()]
        public virtual void TestCleanup()
        {
            this.randomDataUnderTest = null;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Encrypt_Test_Null_Array()
        {
            //Arrange
            byte[] expected = null;

            //Act
            this.randomDataUnderTest.Encrypt(ref expected);

            //Assert
        }

        [TestMethod]
        public void Encrypt_Test_Returns_Encrypted_Array_That_Has_One_Element()
        {
            //Arrange
            int actual;
            int expected = 1;
            byte[] injected = new byte[0];

            //Act
            actual = this.randomDataUnderTest.Encrypt(ref injected).Length;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Decrypt_Test_Null_Array()
        {
            //Arrange
            byte[] expected = null;

            //Act
            this.randomDataUnderTest.Decrypt(ref expected);

            //Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Decrypt_Test_Throws_Error_If_EncryptedLength_Is_More_Than_Argument_Array_Length()
        {
            //Arrange
            byte[] injected = new byte[this.randomDataUnderTest.EncryptedLength - 1];

            for (int i = 0; i < injected.Length; i++)
                injected[i] = Convert.ToByte(Convert.ToInt32('a') + i);

            //Act
            this.randomDataUnderTest.Decrypt(ref injected);

            //Assert
        }

        [TestMethod]
        public void Decrypt_Test_Non_Empty_Array_Returns_Decrypted_Array_That_Is_Empty()
        {
            //Arrange
            int actual;
            int expected = 0;
            byte[] injected = new byte[this.randomDataUnderTest.EncryptedLength];

            for (int i = 0; i < injected.Length; i++)
                injected[i] = Convert.ToByte(Convert.ToInt32('a') + i);

            //Act
            actual = this.randomDataUnderTest.Decrypt(ref injected).Length;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Encrypt_Test_Does_Not_Remove_Bytes_From_Argument_Array()
        {
            //Arrange
            int actual;
            int expected = 2;
            byte[] injected = new byte[expected + this.randomDataUnderTest.Length];

            for (int i = 0; i < injected.Length; i++)
                injected[i] = Convert.ToByte(Convert.ToInt32('a') + i);

            //Act
            this.randomDataUnderTest.Encrypt(ref injected);

            actual = injected.Length;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Decrypt_Test_Removes_Correct_Number_Of_Bytes_From_Argument_Array()
        {
            //Arrange
            int actual;
            int expected = 2;
            byte[] injected = new byte[expected + this.randomDataUnderTest.EncryptedLength];

            for (int i = 0; i < injected.Length; i++)
                injected[i] = Convert.ToByte(Convert.ToInt32('a') + i);

            //Act
            this.randomDataUnderTest.Decrypt(ref injected);

            actual = injected.Length;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Decrypt_Test_Removes_Correct_Bytes_From_Argument_Array()
        {
            //Arrange
            byte[] actual = new byte[5];
            byte[] expected = new byte[actual.Length - this.randomDataUnderTest.EncryptedLength];

            for (int i = 0; i < actual.Length; i++)
                actual[i] = Convert.ToByte(Convert.ToInt32('a') + i);

            for (int i = 0; i < expected.Length; i++)
                expected[i] = actual[i + this.randomDataUnderTest.EncryptedLength];

            //Act
            this.randomDataUnderTest.Decrypt(ref actual);

            //Assert
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }
    }
}
