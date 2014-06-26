using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

using DistributedCipher.Framework;
using DistributedCipher.Common;
using System.Collections.Generic;

namespace DistributedCipher.Test.Unit
{
    public abstract class IByteMapTest
    {
        protected IByteMap byteMapUnderTest;
        protected KeyValuePair<byte, byte> lookupKey;
        protected KeyValuePair<byte, byte> lookupKeyNotFound;
        protected KeyValuePair<byte, byte> reverseLookupKey;
        protected KeyValuePair<byte, byte> reverseLookupKeyNotFound;
        
        private TestContext testContextInstance;
        public TestContext TestContext { get { return testContextInstance; } set { testContextInstance = value; } }

        [TestCleanup()]
        public void TestCleanup()
        {
            this.byteMapUnderTest = null;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Initialize_Test_Null_Dictionary()
        {
            //Arrange
            Dictionary<byte, byte> expected = null;

            //Act
            this.byteMapUnderTest = new ByteMap(expected);

            //Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Initialize_Test_Null_List()
        {
            //Arrange
            IList<byte> expected = null;

            //Act
            this.byteMapUnderTest = new ByteMap(expected);

            //Assert
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidMappedKeyException))]
        public void Initialize_Test_Missing_Mapped_Key()
        {
            //Arrange
            Dictionary<byte, byte> expected = new Dictionary<byte, byte>();

            expected[1] = 2;
            expected[2] = 3;

            //Act
            this.byteMapUnderTest = new ByteMap(expected);

            //Assert
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidMappedKeyException))]
        public void Initialize_Test_Duplicate_Mapped_Value()
        {
            //Arrange
            Dictionary<byte, byte> expected = new Dictionary<byte, byte>();

            expected[1] = 2;
            expected[2] = 1;
            expected[3] = 2;

            //Act
            this.byteMapUnderTest = new ByteMap(expected);

            //Assert
        }

        [TestMethod]
        public void Initialize_Test_Properly_Mapped()
        {
            //Arrange
            Dictionary<byte, byte> expected = new Dictionary<byte, byte>();

            expected[1] = 2;
            expected[2] = 3;
            expected[3] = 1;

            //Act
            this.byteMapUnderTest = new ByteMap(expected);

            //Assert
        }

        [TestMethod]
        public void Lookup_Test_Key_Found()
        {
            //Arrange
            byte actual;
            byte expected = this.lookupKey.Value;
            byte bite = this.lookupKey.Key;

            //Act
            actual = this.byteMapUnderTest.Lookup(bite);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Lookup_Test_Key_Not_Found_Throws_Error()
        {
            //Arrange
            byte actual;
            byte expected = this.lookupKeyNotFound.Key;

            //Act
            actual = this.byteMapUnderTest.Lookup(expected);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ReverseLookup_Test_Byte_Found()
        {
            //Arrange
            byte actual;
            byte bite = this.reverseLookupKey.Key;
            byte expected = this.reverseLookupKey.Value;

            //Act
            actual = this.byteMapUnderTest.ReverseLookup(bite);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ReverseLookup_Test_Byte_Not_Found_Throws_Error()
        {
            //Arrange
            byte actual;
            byte expected = this.reverseLookupKeyNotFound.Key;

            //Act
            actual = this.byteMapUnderTest.ReverseLookup(expected);

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
