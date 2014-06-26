using System;
using System.Collections.Generic;
using System.Data.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

using DistributedCipher.Framework;

namespace DistributedCipher.Test.Unit
{
    public abstract class IByteSetRepositoryTest
    {
        protected IByteSetRepository byteSetRepositoryUnderTest;

        private TestContext testContextInstance;
        public TestContext TestContext { get { return testContextInstance; } set { testContextInstance = value; } }

        [TestCleanup()]
        public void TestCleanup()
        {
            this.byteSetRepositoryUnderTest = null;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Delete_Test_Null_ByteSet()
        {
            //Arrange
            IByteSet expected = null;

            //Act
            this.byteSetRepositoryUnderTest.Delete(expected);

            //Assert
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void Delete_Test_Throws_Error_When_ByteSet_Is_Not_Found()
        {
            //Arrange
            IByteSet expected = MockRepository.GenerateStub<IByteSet>();
            expected.Stub(e => e.ID).Return(Guid.NewGuid());

            //Act
            this.byteSetRepositoryUnderTest.Delete(expected);

            //Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Exists_Test_Null_ByteSet()
        {
            //Arrange
            IByteSet expected = null;

            //Act
            this.byteSetRepositoryUnderTest.Exists(expected);

            //Assert
        }

        [TestMethod]
        public void Exists_Test_Returns_Empty_Guid_When_Deleted()
        {
            //Arrange
            Guid actual;
            Guid expected = Guid.Empty;
            IByteSet byteSet = MockRepository.GenerateStub<IByteSet>();
            byteSet.Stub(e => e.ID).Return(Guid.NewGuid());

            this.byteSetRepositoryUnderTest.Save(byteSet);

            if (this.byteSetRepositoryUnderTest.Exists(byteSet) == Guid.Empty)
                Assert.Inconclusive("Could not save to setup for delete.");

            //Act
            this.byteSetRepositoryUnderTest.Delete(byteSet);

            actual = this.byteSetRepositoryUnderTest.Exists(byteSet);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Exists_Test_Returns_Empty_Guid_When_Not_Found()
        {
            //Arrange
            Guid actual;
            Guid expected = Guid.Empty;
            IByteSet byteSet = MockRepository.GenerateStub<IByteSet>();

            //Act
            actual = this.byteSetRepositoryUnderTest.Exists(byteSet);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Exists_Test_Returns_Guid_When_Found()
        {
            //Arrange
            Guid actual;
            Guid expected = Guid.NewGuid();
            IByteSet byteSet = MockRepository.GenerateStub<IByteSet>();
            byteSet.Stub(e => e.ID).Return(expected);

            this.byteSetRepositoryUnderTest.Save(byteSet);

            //Act
            actual = this.byteSetRepositoryUnderTest.Exists(byteSet);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Exists_Test_Returns_Correct_Guid_When_ByteIndex_IDs_Dont_Match_But_Data_Does()
        {
            //Arrange
            Guid actual;
            Guid expected = Guid.NewGuid();

            IList<byte> data = new List<byte>() { Convert.ToByte('a'), Convert.ToByte('b'), Convert.ToByte('c') };
            IList<byte> nonMatchingData = new List<byte>() { Convert.ToByte('a'), Convert.ToByte('b'), Convert.ToByte('d') };

            IByteIndex matchingByteIndex = MockRepository.GenerateStub<IByteIndex>();
            matchingByteIndex.Stub(b => b.ID).Return(expected);
            matchingByteIndex.Stub(b => b.Data).Return(data);

            IByteIndex nonMatchingByteIndex = MockRepository.GenerateStub<IByteIndex>();
            nonMatchingByteIndex.Stub(b => b.ID).Return( Guid.NewGuid());
            nonMatchingByteIndex.Stub(b => b.Data).Return(nonMatchingData);

            IByteIndex searchingByteIndex = MockRepository.GenerateStub<IByteIndex>();
            searchingByteIndex.Stub(b => b.ID).Return( Guid.NewGuid());
            searchingByteIndex.Stub(b => b.Data).Return(data);

            this.byteSetRepositoryUnderTest.Save(matchingByteIndex);
            this.byteSetRepositoryUnderTest.Save(nonMatchingByteIndex);

            //Act
            actual = this.byteSetRepositoryUnderTest.Exists(searchingByteIndex);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Exists_Test_Returns_Correct_Guid_When_ByteMap_IDs_Dont_Match_But_Data_Does()
        {
            //Arrange
            Guid actual;
            Guid expected = Guid.NewGuid();

            Dictionary<byte, byte> data = new Dictionary<byte, byte>();
            data[Convert.ToByte('a')] = Convert.ToByte('b');
            data[Convert.ToByte('b')] = Convert.ToByte('c');
            data[Convert.ToByte('c')] = Convert.ToByte('d');

            Dictionary<byte, byte> nonMatchingData = new Dictionary<byte, byte>();
            nonMatchingData[Convert.ToByte('a')] = Convert.ToByte('b');
            nonMatchingData[Convert.ToByte('b')] = Convert.ToByte('c');
            nonMatchingData[Convert.ToByte('c')] = Convert.ToByte('e');

            IByteMap matchingByteMap = MockRepository.GenerateStub<IByteMap>();
            matchingByteMap.Stub(b => b.ID).Return( expected);
            matchingByteMap.Stub(b => b.Data).Return(data);

            IByteMap nonMatchingByteMap = MockRepository.GenerateStub<IByteMap>();
            nonMatchingByteMap.Stub(b => b.ID).Return(Guid.NewGuid());
            nonMatchingByteMap.Stub(b => b.Data).Return(nonMatchingData);

            IByteMap searchingByteMap = MockRepository.GenerateStub<IByteMap>();
            searchingByteMap.Stub(b => b.ID).Return( Guid.NewGuid());
            searchingByteMap.Stub(b => b.Data).Return(data);

            this.byteSetRepositoryUnderTest.Save(matchingByteMap);
            this.byteSetRepositoryUnderTest.Save(nonMatchingByteMap);

            //Act
            actual = this.byteSetRepositoryUnderTest.Exists(searchingByteMap);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Exists_Test_Returns_Proper_Guid_When_Found()
        {
            //Arrange
            Guid actual;
            Guid expected = Guid.NewGuid();

            IByteSet first = MockRepository.GenerateStub<IByteSet>();
            IByteSet second = MockRepository.GenerateStub<IByteSet>();
            IByteSet third = MockRepository.GenerateStub<IByteSet>();

            first.Stub(f => f.ID).Return( Guid.NewGuid());
            second.Stub(s => s.ID).Return( expected);
            third.Stub(t => t.ID).Return( Guid.NewGuid());

            this.byteSetRepositoryUnderTest.Save(first);
            this.byteSetRepositoryUnderTest.Save(second);
            this.byteSetRepositoryUnderTest.Save(third);

            //Act
            actual = this.byteSetRepositoryUnderTest.Exists(second);

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
            this.byteSetRepositoryUnderTest.Find(expected);

            //Assert
        }

        [TestMethod]
        public void Find_Test_Returns_Null_When_Not_Found()
        {
            //Arrange
            IByteSet actual;
            IByteSet expected = null;
            IByteSet byteSet = MockRepository.GenerateStub<IByteSet>();
            byteSet.Stub(e => e.ID).Return(Guid.NewGuid());
            
            //Act
            actual = this.byteSetRepositoryUnderTest.Find(byteSet.ID);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Find_Test_Returns_ByteSet_When_Found()
        {
            //Arrange
            IByteSet actual;
            IByteSet expected = MockRepository.GenerateStub<IByteSet>();
            expected.Stub(e => e.ID).Return(Guid.NewGuid());

            this.byteSetRepositoryUnderTest.Save(expected);

            //Act
            actual = this.byteSetRepositoryUnderTest.Find(expected.ID);

            //Assert
            Assert.AreEqual(expected.ID, actual.ID);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Save_Test_Null_ByteSet()
        {
            //Arrange
            IByteSet expected = null;

            //Act
            this.byteSetRepositoryUnderTest.Save(expected);

            //Assert
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateKeyException))]
        public void Save_Test_Throws_Error_When_ByteSet_Already_Saved()
        {
            //Arrange
            IByteSet expected = MockRepository.GenerateStub<IByteSet>();
            expected.Stub(e => e.ID).Return(Guid.NewGuid());

            this.byteSetRepositoryUnderTest.Save(expected);

            //Act
            this.byteSetRepositoryUnderTest.Save(expected);

            //Assert
        }
    }
}
