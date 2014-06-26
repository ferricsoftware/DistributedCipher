using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

using DistributedCipher.Framework;

namespace DistributedCipher.Test.Unit
{
    public abstract class ICipherTypeTest
    {
        protected ICipherType cipherTypeUnderTest;

        private TestContext testContextInstance;
        public TestContext TestContext { get { return testContextInstance; } set { testContextInstance = value; } }

        [TestCleanup()]
        public void TestCleanup()
        {
            this.cipherTypeUnderTest = null;
        }

        [TestMethod]
        public void Decrypt_Encrypted_Array_Test_Arrays_Match()
        {
            //Arrange
            byte[] actual;
            byte[] expected = FerricHelper.ConvertToByteArray(FerricHelper.KeyboardCharacters);

            //Act
            actual = this.cipherTypeUnderTest.Decrypt(this.cipherTypeUnderTest.Encrypt(expected));

            //Assert
            Assert.AreEqual(expected.Length, actual.Length);

            for (int i = 0; i < actual.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void Decrypt_Test_Empty_Array_Returns_Decrypted_Array_That_Is_Empty()
        {
            //Arrange
            byte[] actual;
            byte[] expected = new byte[0];

            //Act
            actual = this.cipherTypeUnderTest.Decrypt(expected);

            //Assert
            Assert.AreEqual(0, actual.Length);
        }

        [TestMethod]
        public void Decrypt_Test_Empty_Array_Returns_Decrypted_Array_That_Isnt_Null()
        {
            //Arrange
            byte[] actual;
            byte[] expected = new byte[0];

            //Act
            actual = this.cipherTypeUnderTest.Decrypt(expected);

            //Assert
            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public void Decrypt_Test_Non_Empty_Array_Returns_Decrypted_Array_That_Isnt_Empty()
        {
            //Arrange
            byte[] actual;
            byte[] expected = new byte[1] { Convert.ToByte('a') };

            //Act
            actual = this.cipherTypeUnderTest.Decrypt(expected);

            //Assert
            Assert.AreNotEqual(0, actual.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Decrypt_Test_Null_Array()
        {
            //Arrange
            byte[] expected = null;

            //Act
            this.cipherTypeUnderTest.Decrypt(expected);

            //Assert
        }

        [TestMethod]
        public void Encrypt_Test_Empty_Array_Returns_Encrypted_Array_That_Is_Empty()
        {
            //Arrange
            byte[] actual;
            byte[] expected = new byte[0];

            //Act
            actual = this.cipherTypeUnderTest.Encrypt(expected);

            //Assert
            Assert.AreEqual(0, actual.Length);
        }

        [TestMethod]
        public void Encrypt_Test_Empty_Array_Returns_Encrypted_Array_That_Isnt_Null()
        {
            //Arrange
            byte[] actual;
            byte[] expected = new byte[0];

            //Act
            actual = this.cipherTypeUnderTest.Encrypt(expected);

            //Assert
            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public void Encrypt_Test_Non_Empty_Array_Returns_Decrypted_Array_That_Isnt_Empty()
        {
            //Arrange
            byte[] actual;
            byte[] expected = new byte[1] { Convert.ToByte('a') };

            //Act
            actual = this.cipherTypeUnderTest.Encrypt(expected);

            //Assert
            Assert.AreNotEqual(0, actual.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Encrypt_Test_Null_Array()
        {
            //Arrange
            byte[] expected = null;

            //Act
            this.cipherTypeUnderTest.Encrypt(expected);

            //Assert
        }

        //[TestMethod]
        //public void Set_DistributedCipher_Test_Calls_Register_On_DistributedCipher()
        //{
        //    //Arrange
        //    IDistributedCipher firstCheck = MockRepository.GenerateMock<IDistributedCipher>();
        //    IDistributedCipher secondCheck = MockRepository.GenerateMock<IDistributedCipher>();

        //    firstCheck.Expect(dc => dc.Register(Arg<IByteIndex>.Is.Anything)).Repeat.Once();
        //    secondCheck.Expect(dc => dc.Register(Arg<IByteMap>.Is.Anything)).Repeat.Once();

        //    //Act
        //    this.cipherTypeUnderTest.DistributedCipher = firstCheck;
        //    this.cipherTypeUnderTest.DistributedCipher = secondCheck;

        //    //Assert
        //    try { firstCheck.VerifyAllExpectations(); }
        //    catch { secondCheck.VerifyAllExpectations(); }
        //}
    }
}
