using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using DistributedCipher.Common;
using DistributedCipher.Framework;

namespace DistributedCipher.Test.Unit.Common
{
    [TestClass]
    public class ReplacementMapTest : ICipherTypeTest
    {
        [TestInitialize()]
        public void TestInitialize()
        {
            IByteMap byteMap = new ByteMap(FerricHelper.KeyboardCharacters);

            this.cipherTypeUnderTest = new ReplacementMap(byteMap);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Initialize_Test_Null_ByteMap()
        {
            //Arrange
            IByteMap expected = null;

            //Act
            this.cipherTypeUnderTest = new ReplacementMap(expected);

            //Assert
        }
    }
}
