using System;
using System.Collections.Generic;
using System.Data.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using DistributedCipher.Framework;

namespace DistributedCipher.Test.Unit
{
    public abstract class IByteSetRepositoryTest
    {
        protected IByteSet byteSet1;
        protected IByteSet byteSet2;
        protected IByteSet byteSet3;
        protected IByteSetRepository byteSetRepositoryUnderTest;

        private TestContext testContextInstance;
        public TestContext TestContext { get { return testContextInstance; } set { testContextInstance = value; } }

        public virtual void TestInitialize()
        {
            if (this.byteSet1 == null)
                throw new NullReferenceException("ByteSet1");
            if (this.byteSet2 == null)
                throw new NullReferenceException("ByteSet2");
            if (this.byteSet3 == null)
                throw new NullReferenceException("ByteSet3");
            if (this.byteSetRepositoryUnderTest == null)
                throw new NullReferenceException("ByteSetRepositoryUnderTest");

            this.byteSetRepositoryUnderTest.Save(this.byteSet2);
            this.byteSetRepositoryUnderTest.Save(this.byteSet3);
        }

        [TestCleanup()]
        public void TestCleanup()
        {
            if (this.byteSetRepositoryUnderTest.Exists(this.byteSet1) != Guid.Empty)
                this.byteSetRepositoryUnderTest.Delete(this.byteSet1);
            this.byteSet1 = null;

            if (this.byteSetRepositoryUnderTest.Exists(this.byteSet2) != Guid.Empty)
                this.byteSetRepositoryUnderTest.Delete(this.byteSet2);
            this.byteSet2 = null;

            if (this.byteSetRepositoryUnderTest.Exists(this.byteSet3) != Guid.Empty)
                this.byteSetRepositoryUnderTest.Delete(this.byteSet3);
            this.byteSet3 = null;

            this.byteSetRepositoryUnderTest = null;
        }
        
        [TestMethod]
        public void Exists_Test_Returns_Empty_Guid_When_Deleted()
        {
            //Arrange
            Guid actual;
            Guid expected = Guid.Empty;

            this.byteSetRepositoryUnderTest.Save(this.byteSet1);

            if (this.byteSetRepositoryUnderTest.Exists(this.byteSet1) == Guid.Empty)
                Assert.Inconclusive("Could not save to setup for delete.");

            //Act
            this.byteSetRepositoryUnderTest.Delete(this.byteSet1);

            actual = this.byteSetRepositoryUnderTest.Exists(this.byteSet1);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Exists_Test_Returns_Empty_Guid_When_Not_Found()
        {
            //Arrange
            Guid actual;
            Guid expected = Guid.Empty;

            //Act
            actual = this.byteSetRepositoryUnderTest.Exists(this.byteSet1);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Exists_Test_Returns_Guid_When_Found()
        {
            //Arrange
            Guid actual;
            Guid expected = this.byteSet1.ID;

            this.byteSetRepositoryUnderTest.Save(this.byteSet1);

            //Act
            actual = this.byteSetRepositoryUnderTest.Exists(this.byteSet1);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Exists_Test_Returns_Proper_Guid_When_Found()
        {
            //Arrange
            Guid actual;
            Guid expected = this.byteSet2.ID;

            this.byteSetRepositoryUnderTest.Save(this.byteSet1);

            //Act
            actual = this.byteSetRepositoryUnderTest.Exists(this.byteSet2);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Find_Test_Returns_Null_When_Not_Found()
        {
            //Arrange
            IByteSet actual;
            IByteSet expected = null;

            //Act
            actual = this.byteSetRepositoryUnderTest.Find(Guid.NewGuid());

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Find_Test_Returns_ByteSet_When_Found()
        {
            //Arrange
            IByteSet actual;
            IByteSet expected = this.byteSet1;

            this.byteSetRepositoryUnderTest.Save(expected);

            //Act
            actual = this.byteSetRepositoryUnderTest.Find(expected.ID);

            //Assert
            Assert.AreEqual(expected.ID, actual.ID);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateKeyException))]
        public void Save_Test_Throws_Error_When_ByteSet_Already_Saved()
        {
            //Arrange
            IByteSet expected = this.byteSet1;
            
            this.byteSetRepositoryUnderTest.Save(expected);

            //Act
            this.byteSetRepositoryUnderTest.Save(expected);

            //Assert
        }
    }
}
