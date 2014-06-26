using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

using DistributedCipher.Framework;

namespace DistributedCipher.Test.Unit
{
    public abstract class IHeaderTest
    {
        protected IHeader headerUnderTest;
        protected IDistributedCipher distributedCipher;
        protected int numberOfCiphers;

        private TestContext testContextInstance;
        public TestContext TestContext { get { return testContextInstance; } set { testContextInstance = value; } }

        [TestInitialize()]
        public virtual void TestInitialize()
        {
            IList<ICipher> ciphers = new List<ICipher>();
            for (int i = 0; i < this.numberOfCiphers; i++)
                ciphers.Add(MockRepository.GenerateMock<ICipher>());

            this.distributedCipher = MockRepository.GenerateMock<IDistributedCipher>();
            this.distributedCipher.Stub(dc => dc.Ciphers).Return(ciphers);
        }

        [TestCleanup()]
        public void TestCleanup()
        {
            this.distributedCipher = null;
            this.headerUnderTest = null;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Decrypt_Test_Null_Array()
        {
            //Arrange
            byte[] expected = null;

            //Act
            this.headerUnderTest.Decrypt(ref expected);

            //Assert
        }

        [TestMethod]
        public void Decrypt_Test_Properly_Decrypts_Encrypted_Bytes()
        {
            //Arrange
            int actual;
            int expected = 10;

            byte[] bytes = this.headerUnderTest.Encrypt(expected);

            //Act
            actual = this.headerUnderTest.Decrypt(ref bytes);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Encrypt_Test_Returned_Array_Is_Correct_Length()
        {
            //Arrange
            int expected = 10;
            byte[] actual = null;

            //Act
            actual = this.headerUnderTest.Encrypt(expected);

            //Assert
            Assert.AreEqual(this.headerUnderTest.EncryptedLength, actual.Length);
        }

        [TestMethod]
        public void Encrypt_Test_Returned_Array_Is_Not_Null()
        {
            //Arrange
            int expected = 10;
            byte[] actual = null;

            //Act
            actual = this.headerUnderTest.Encrypt(expected);

            //Assert
            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public void Encrypt_Test_With_Maximum_Offset_Minus_One()
        {
            //Arrange
            int expected = this.numberOfCiphers - 1;
            byte[] actual = null;

            //Act
            actual = this.headerUnderTest.Encrypt(expected);

            //Assert
            Assert.IsNotNull(actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Encrypt_Test_With_Maximum_Offset_Plus_One()
        {
            //Arrange
            int expected = this.numberOfCiphers;
            
            //Act
            this.headerUnderTest.Encrypt(expected);

            //Assert
        }
    }
}
