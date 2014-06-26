using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

using DistributedCipher.Framework;

namespace DistributedCipher.Test.Unit
{
    public abstract class IByteIndexTest
    {
        protected IByteIndex byteIndexUnderTest;
        protected KeyValuePair<int, byte> keyValueNotFound;
        protected KeyValuePair<int, byte> maximumIndex;
        protected KeyValuePair<int, byte> minimumIndex;

        private TestContext testContextInstance;
        public TestContext TestContext { get { return testContextInstance; } set { testContextInstance = value; } }

        [TestCleanup()]
        public void TestCleanup()
        {
            this.byteIndexUnderTest = null;
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Lookup_Test_Minimum_Index_Minus_One_Throws_Error()
        {
            //Arrange
            int index = this.minimumIndex.Key - 1;

            //Act
            this.byteIndexUnderTest.Lookup(index);

            //Assert
        }

        [TestMethod]
        public void Lookup_Test_Minimum_Index()
        {
            //Arrange
            byte actual;
            byte expected = this.minimumIndex.Value;
            int index = this.minimumIndex.Key;

            //Act
            actual = this.byteIndexUnderTest.Lookup(index);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Lookup_Test_Maximum_Index()
        {
            //Arrange
            byte actual;
            byte expected = this.maximumIndex.Value;
            int index = this.maximumIndex.Key;

            //Act
            actual = this.byteIndexUnderTest.Lookup(index);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Lookup_Test_Maximum_Index_Plus_One_Throws_Error()
        {
            //Arrange
            int index = this.maximumIndex.Key + 1;

            //Act
            this.byteIndexUnderTest.Lookup(index);

            //Assert
        }

        [TestMethod]
        public void ReverseLookup_Test_Byte_Found()
        {
            //Arrange
            int actual;
            byte bite = this.minimumIndex.Value;
            int expected = this.minimumIndex.Key;

            //Act
            actual = this.byteIndexUnderTest.ReverseLookup(bite);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void ReverseLookup_Test_Byte_Not_Found_Throws_Error()
        {
            //Arrange
            byte expected = this.keyValueNotFound.Value;

            //Act
            this.byteIndexUnderTest.ReverseLookup(expected);

            //Assert
        }
    }
}
