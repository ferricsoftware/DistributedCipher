using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

using DistributedCipher.Framework;

namespace DistributedCipher.Test.Unit
{
    public abstract class ICipherTest
    {
        protected ICipher cipherUnderTest;
        protected ICipherType cipherType;

        private TestContext testContextInstance;
        public TestContext TestContext { get { return testContextInstance; } set { testContextInstance = value; } }

        [TestInitialize()]
        public virtual void TestInitialize()
        {
            this.cipherType = MockRepository.GenerateMock<ICipherType>();
        }

        [TestCleanup()]
        public void TestCleanup()
        {
            this.cipherUnderTest = null;
            this.cipherType = null;
        }

        [TestMethod]
        public void Decrypt_Test_Calls_Decrypt_On_Cipher_Type_With_Array()
        {
            //Arrange
            byte[] injected = new byte[this.cipherUnderTest.EncryptedLength];

            for (int i = 0; i < injected.Length; i++)
                injected[i] = Convert.ToByte(Convert.ToInt32('a') + i);

            //Act
            this.cipherUnderTest.Decrypt(ref injected);

            //Assert
            this.cipherType.AssertWasCalled(ct => ct.Decrypt(Arg<byte[]>.Is.Anything));
        }

        [TestMethod]
        public void Decrypt_Test_Non_Empty_Array_Returns_Decrypted_Array_That_Isnt_Empty()
        {
            //Arrange
            int actual;
            int expected = 0;
            byte[] injected = new byte[this.cipherUnderTest.EncryptedLength];

            for (int i = 0; i < injected.Length; i++)
                injected[i] = Convert.ToByte(Convert.ToInt32('a') + i);

            this.cipherType.Stub(ct => { ct.Decrypt(Arg<byte[]>.Is.Anything); })
                .Return(injected);

            //Act
            actual = this.cipherUnderTest.Decrypt(ref injected).Length;

            //Assert
            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Decrypt_Test_Null_Array()
        {
            //Arrange
            byte[] expected = null;

            //Act
            this.cipherUnderTest.Decrypt(ref expected);

            //Assert
        }

        [TestMethod]
        public void Decrypt_Test_Removes_Correct_Bytes_From_Argument_Array()
        {
            //Arrange
            byte[] actual = new byte[5];
            byte[] expected = new byte[actual.Length - this.cipherUnderTest.EncryptedLength];

            for (int i = 0; i < actual.Length; i++)
                actual[i] = Convert.ToByte(Convert.ToInt32('a') + i);

            for (int i = 0; i < expected.Length; i++)
                expected[i] = actual[this.cipherUnderTest.EncryptedLength + i];

            this.cipherType.Stub(ct => { ct.Decrypt(Arg<byte[]>.Is.Anything); })
                .Return(actual);

            //Act
            this.cipherUnderTest.Decrypt(ref actual);

            //Assert
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void Decrypt_Test_Removes_Correct_Number_Of_Bytes_From_Argument_Array()
        {
            //Arrange
            int actual;
            int expected = 2;
            byte[] injected = new byte[expected + this.cipherUnderTest.EncryptedLength];
            byte[] returned = new byte[expected];

            for (int i = 0; i < injected.Length; i++)
                injected[i] = Convert.ToByte(Convert.ToInt32('a') + i);

            this.cipherType.Stub(ct => { ct.Decrypt(Arg<byte[]>.Is.Anything); })
                .Return(returned);

            //Act
            actual = this.cipherUnderTest.Decrypt(ref injected).Length;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Decrypt_Test_Returns_Decrypted_Byte_Array()
        {
            //Arrange
            byte[] actual;
            byte[] expected = new byte[1] { Convert.ToByte('a') };
            byte[] injected = new byte[this.cipherUnderTest.EncryptedLength + 1];

            for (int i = 0; i < injected.Length; i++)
                injected[i] = Convert.ToByte(Convert.ToInt32('a') + i);

            this.cipherType.Stub(ct => ct.Decrypt(Arg<byte[]>.Is.Anything))
                .Return(expected);

            //Act
            actual = this.cipherUnderTest.Decrypt(ref injected);

            //Assert
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Decrypt_Test_Throws_Error_If_EncryptedLength_Is_More_Than_Argument_Array_Length()
        {
            //Arrange
            byte[] injected = new byte[this.cipherUnderTest.EncryptedLength - 1];

            for (int i = 0; i < injected.Length; i++)
                injected[i] = Convert.ToByte(Convert.ToInt32('a') + i);

            //Act
            this.cipherUnderTest.Decrypt(ref injected);

            //Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Decrypt_Test_Throws_Error_If_Length_Is_More_Than_Argument_Array_Length()
        {
            //Arrange
            byte[] injected = new byte[this.cipherUnderTest.EncryptedLength - 1];

            for (int i = 0; i < injected.Length; i++)
                injected[i] = Convert.ToByte(Convert.ToInt32('a') + i);

            //Act
            this.cipherUnderTest.Decrypt(ref injected);

            //Assert
        }

        [TestMethod]
        public void Encrypt_Test_Calls_Encrypt_On_Cipher_Type_With_Array()
        {
            //Arrange
            byte[] injected = new byte[this.cipherUnderTest.Length];

            for (int i = 0; i < injected.Length; i++)
                injected[i] = Convert.ToByte(Convert.ToInt32('a') + i);

            //Act
            this.cipherUnderTest.Encrypt(ref injected);

            //Assert
            this.cipherType.AssertWasCalled(ct => ct.Encrypt(Arg<byte[]>.Is.Anything));
        }

        [TestMethod]
        public void Encrypt_Test_Non_Empty_Array_Returns_Decrypted_Array_That_Isnt_Empty()
        {
            //Arrange
            int actual;
            int expected = 0;
            byte[] injected = new byte[this.cipherUnderTest.Length];

            for (int i = 0; i < injected.Length; i++)
                injected[i] = Convert.ToByte(Convert.ToInt32('a') + i);

            this.cipherType.Stub(ct => { ct.Encrypt(Arg<byte[]>.Is.Anything); })
                .Return(injected);

            //Act
            actual = this.cipherUnderTest.Encrypt(ref injected).Length;

            //Assert
            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Encrypt_Test_Null_Array()
        {
            //Arrange
            byte[] expected = null;

            //Act
            this.cipherUnderTest.Encrypt(ref expected);

            //Assert
        }

        [TestMethod]
        public void Encrypt_Test_Removes_Correct_Bytes_From_Argument_Array()
        {
            //Arrange
            byte[] actual = new byte[5];
            byte[] expected = new byte[actual.Length - this.cipherUnderTest.Length];

            for (int i = 0; i < actual.Length; i++)
                actual[i] = Convert.ToByte(Convert.ToInt32('a') + i);

            for (int i = 0; i < actual.Length - this.cipherUnderTest.Length; i++)
                expected[i] = actual[i + this.cipherUnderTest.Length];

            this.cipherType.Stub(ct => { ct.Encrypt(Arg<byte[]>.Is.Anything); })
                .Return(actual);

            //Act
            this.cipherUnderTest.Encrypt(ref actual);

            //Assert
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void Encrypt_Test_Removes_Correct_Number_Of_Bytes_From_Argument_Array()
        {
            //Arrange
            int actual;
            int expected = 2;
            byte[] injected = new byte[expected + this.cipherUnderTest.Length];

            for (int i = 0; i < injected.Length; i++)
                injected[i] = Convert.ToByte(Convert.ToInt32('a') + i);

            this.cipherType.Stub(ct => { ct.Encrypt(Arg<byte[]>.Is.Anything); })
                .Return(injected);

            //Act
            this.cipherUnderTest.Encrypt(ref injected);

            actual = injected.Length;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Encrypt_Test_Returns_Encrypted_Byte_Array()
        {
            //Arrange
            byte[] actual;
            byte[] expected = new byte[1] { Convert.ToByte('a') };
            byte[] injected = new byte[this.cipherUnderTest.Length];

            for (int i = 0; i < injected.Length; i++)
                injected[i] = Convert.ToByte(Convert.ToInt32('a') + i);

            this.cipherType.Stub(ct => ct.Encrypt(Arg<byte[]>.Is.Anything))
                .Return(expected);

            //Act
            actual = this.cipherUnderTest.Encrypt(ref injected);

            //Assert
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Encrypt_Test_Throws_Error_If_Length_Is_More_Than_Argument_Array_Length()
        {
            //Arrange
            byte[] injected = new byte[this.cipherUnderTest.Length - 1];

            for (int i = 0; i < injected.Length; i++)
                injected[i] = Convert.ToByte('a');

            //Act
            this.cipherUnderTest.Encrypt(ref injected);

            //Assert
        }
    }
}
