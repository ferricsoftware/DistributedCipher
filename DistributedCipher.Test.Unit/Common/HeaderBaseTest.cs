using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

using DistributedCipher.Common;
using DistributedCipher.Framework;

namespace DistributedCipher.Test.Unit.Common
{
    public class HeaderBaseTest : IHeaderTest
    {
        [TestMethod]
        public void Initialization_Test_Using_Existing_Key_Properly_Decrypts_Encrypted_Array()
        {
            //Arrange
            int actual;
            byte[] bytes;
            int expected = 10;

            bytes = this.headerUnderTest.Encrypt(expected);

            IHeader secondDistributedHeader = new Header(
                this.headerUnderTest.ByteIndex,
                this.headerUnderTest.Key);

            //Act
            actual = secondDistributedHeader.Decrypt(ref bytes);

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
