using System;
using System.Collections.Generic;
using System.Data.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using DistributedCipher.Framework;

namespace DistributedCipher.Test.Unit
{
    public abstract class IFerricNodeRepositoryTest
    {
        protected IFerricNode ferricNode1;
        protected IFerricNode ferricNode2;
        protected IFerricNode ferricNode3;
        protected IFerricNode ferricNode4;
        protected IFerricNodeRepository ferricNodeRepositoryUnderTest;

        private TestContext testContextInstance;
        public TestContext TestContext { get { return testContextInstance; } set { testContextInstance = value; } }

        public virtual void TestInitialize()
        {
            if (this.ferricNode1 == null)
                throw new NullReferenceException("DistributedCipher1");
            if (this.ferricNode2 == null)
                throw new NullReferenceException("DistributedCipher2");
            if (this.ferricNode3 == null)
                throw new NullReferenceException("DistributedCipher3");
            if (this.ferricNode4 == null)
                throw new NullReferenceException("DistributedCipher4");
            if (this.ferricNodeRepositoryUnderTest == null)
                throw new NullReferenceException("DistributedCipherRepositoryUnderTest");

            this.ferricNodeRepositoryUnderTest.Save(this.ferricNode2);
            this.ferricNodeRepositoryUnderTest.Save(this.ferricNode3);
            this.ferricNodeRepositoryUnderTest.Save(this.ferricNode4);
        }

        [TestCleanup()]
        public void TestCleanup()
        {
            this.ferricNode1 = null;
            this.ferricNode2 = null;
            this.ferricNode3 = null;
            this.ferricNode4 = null;
            this.ferricNodeRepositoryUnderTest = null;
        }
        
        [TestMethod]
        public void Exists_Test_Returns_Empty_Guid_When_Deleted()
        {
            //Arrange
            Guid actual;
            Guid expected = Guid.Empty;

            this.ferricNodeRepositoryUnderTest.Save(this.ferricNode1);

            if (this.ferricNodeRepositoryUnderTest.Exists(this.ferricNode1) == Guid.Empty)
                Assert.Inconclusive("Could not save to setup for delete.");

            //Act
            this.ferricNodeRepositoryUnderTest.Delete(this.ferricNode1);

            actual = this.ferricNodeRepositoryUnderTest.Exists(this.ferricNode1);

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
            actual = this.ferricNodeRepositoryUnderTest.Exists(this.ferricNode1);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Exists_Test_Returns_Guid_When_Found()
        {
            //Arrange
            Guid actual;
            Guid expected = this.ferricNode1.ID;

            this.ferricNodeRepositoryUnderTest.Save(this.ferricNode1);

            //Act
            actual = this.ferricNodeRepositoryUnderTest.Exists(this.ferricNode1);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Exists_Test_Returns_Proper_Guid_When_Found()
        {
            //Arrange
            Guid actual;
            Guid expected = this.ferricNode2.ID;

            this.ferricNodeRepositoryUnderTest.Save(this.ferricNode1);

            //Act
            actual = this.ferricNodeRepositoryUnderTest.Exists(this.ferricNode2);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Find_Test_Returns_Null_When_Not_Found()
        {
            //Arrange
            IFerricNode actual;
            IFerricNode expected = null;

            //Act
            actual = this.ferricNodeRepositoryUnderTest.Find(Guid.NewGuid());

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Find_Test_Returns_ByteSet_When_Found()
        {
            //Arrange
            IFerricNode actual;
            IFerricNode expected = this.ferricNode1;

            this.ferricNodeRepositoryUnderTest.Save(expected);

            //Act
            actual = this.ferricNodeRepositoryUnderTest.Find(expected.ID);

            //Assert
            Assert.AreEqual(expected.ID, actual.ID);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateKeyException))]
        public void Save_Test_Throws_Error_When_ByteSet_Already_Saved()
        {
            //Arrange
            IFerricNode expected = this.ferricNode1;
            
            this.ferricNodeRepositoryUnderTest.Save(expected);

            //Act
            this.ferricNodeRepositoryUnderTest.Save(expected);

            //Assert
        }
    }
}
