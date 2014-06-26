using System;
using System.Collections.Generic;
using System.Data.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

using DistributedCipher.Framework;

namespace DistributedCipher.Test.Unit
{
    public abstract class IFerricNodeRepositoryTest
    {
        protected IFerricNodeRepository ferricNodeRepositoryUnderTest;

        private TestContext testContextInstance;
        public TestContext TestContext { get { return testContextInstance; } set { testContextInstance = value; } }

        [TestCleanup()]
        public void TestCleanup()
        {
            this.ferricNodeRepositoryUnderTest = null;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Delete_Test_Null_FerricNode()
        {
            //Arrange
            IFerricNode expected = null;

            //Act
            this.ferricNodeRepositoryUnderTest.Delete(expected);

            //Assert
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void Delete_Test_Throws_Error_When_FerricNode_Is_Not_Found()
        {
            //Arrange
            IFerricNode expected = MockRepository.GenerateStub<IFerricNode>();
            expected.Stub(e => e.ID).Return(Guid.NewGuid());

            //Act
            this.ferricNodeRepositoryUnderTest.Delete(expected);

            //Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Exists_Test_Null_FerricNode()
        {
            //Arrange
            IFerricNode expected = null;

            //Act
            this.ferricNodeRepositoryUnderTest.Exists(expected);

            //Assert
        }

        [TestMethod]
        public void Exists_Test_Returns_Empty_Guid_When_Deleted()
        {
            //Arrange
            Guid actual;
            Guid expected = Guid.Empty;
            IFerricNode byteSet = MockRepository.GenerateStub<IFerricNode>();
            byteSet.Stub(e => e.ID).Return(Guid.NewGuid());

            this.ferricNodeRepositoryUnderTest.Save(byteSet);

            if (this.ferricNodeRepositoryUnderTest.Exists(byteSet) == Guid.Empty)
                Assert.Inconclusive("Could not save to setup for delete.");

            //Act
            this.ferricNodeRepositoryUnderTest.Delete(byteSet);

            actual = this.ferricNodeRepositoryUnderTest.Exists(byteSet);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Exists_Test_Returns_Empty_Guid_When_Not_Found()
        {
            //Arrange
            Guid actual;
            Guid expected = Guid.Empty;
            IFerricNode byteSet = MockRepository.GenerateStub<IFerricNode>();

            //Act
            actual = this.ferricNodeRepositoryUnderTest.Exists(byteSet);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Exists_Test_Returns_Guid_When_Found()
        {
            //Arrange
            Guid actual;
            Guid expected = Guid.NewGuid();
            IFerricNode byteSet = MockRepository.GenerateStub<IFerricNode>();
            byteSet.Stub(e => e.ID).Return(expected);

            this.ferricNodeRepositoryUnderTest.Save(byteSet);

            //Act
            actual = this.ferricNodeRepositoryUnderTest.Exists(byteSet);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Exists_Test_Returns_Proper_Guid_When_Found()
        {
            //Arrange
            Guid actual;
            Guid expected = Guid.NewGuid();

            IFerricNode first = MockRepository.GenerateStub<IFerricNode>();
            IFerricNode second = MockRepository.GenerateStub<IFerricNode>();
            IFerricNode third = MockRepository.GenerateStub<IFerricNode>();

            first.Stub(f => f.ID).Return( Guid.NewGuid());
            second.Stub(s => s.ID).Return( expected);
            third.Stub(t => t.ID).Return( Guid.NewGuid());

            this.ferricNodeRepositoryUnderTest.Save(first);
            this.ferricNodeRepositoryUnderTest.Save(second);
            this.ferricNodeRepositoryUnderTest.Save(third);

            //Act
            actual = this.ferricNodeRepositoryUnderTest.Exists(second);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Find_Test_Empty_Guid()
        {
            //Arrange
            Guid expected = Guid.Empty;

            //Act
            this.ferricNodeRepositoryUnderTest.Find(expected);

            //Assert
        }

        [TestMethod]
        public void Find_Test_Returns_Null_When_Not_Found()
        {
            //Arrange
            IFerricNode actual;
            IFerricNode expected = null;
            IFerricNode ferricNode = MockRepository.GenerateStub<IFerricNode>();
            ferricNode.Stub(e => e.ID).Return(Guid.NewGuid());
            
            //Act
            actual = this.ferricNodeRepositoryUnderTest.Find(ferricNode.ID);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Find_Test_Returns_FerricNode_When_Found()
        {
            //Arrange
            IFerricNode actual;
            IFerricNode expected = MockRepository.GenerateStub<IFerricNode>();
            expected.Stub(e => e.ID).Return(Guid.NewGuid());

            this.ferricNodeRepositoryUnderTest.Save(expected);

            //Act
            actual = this.ferricNodeRepositoryUnderTest.Find(expected.ID);

            //Assert
            Assert.AreEqual(expected.ID, actual.ID);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Save_Test_Null_FerricNode()
        {
            //Arrange
            IFerricNode expected = null;

            //Act
            this.ferricNodeRepositoryUnderTest.Save(expected);

            //Assert
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateKeyException))]
        public void Save_Test_Throws_Error_When_FerricNode_Already_Saved()
        {
            //Arrange
            IFerricNode expected = MockRepository.GenerateStub<IFerricNode>();
            expected.Stub(e => e.ID).Return(Guid.NewGuid());

            this.ferricNodeRepositoryUnderTest.Save(expected);

            //Act
            this.ferricNodeRepositoryUnderTest.Save(expected);

            //Assert
        }
    }
}
