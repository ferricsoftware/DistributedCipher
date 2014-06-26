using System;
using System.Collections.Generic;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using DistributedCipher.Common;
using DistributedCipher.Framework;

namespace DistributedCipher.Test.Integration
{
    public abstract class IFerricNodeTest
    {
        protected IByteSetRepository byteSetRepository;
        protected IFerricNode ferricNodeUnderTest;
        protected IFerricNodeRepository ferricNodeRepository;
        protected IList<char> injectedList;
        protected IList<byte> largeAmountOfData;
        protected IXmlFactory xmlFactory;

        private TestContext testContextInstance;
        public TestContext TestContext { get { return testContextInstance; } set { testContextInstance = value; } }

        [TestInitialize]
        public virtual void TestInitialize()
        {
            if (this.byteSetRepository == null)
                throw new ArgumentNullException("ByteSetRepository");
            if (this.xmlFactory == null)
                throw new ArgumentNullException("XmlFactory");

            this.ferricNodeRepository = new FerricNodeXmlRepository(this.GetType().ToString(), this.byteSetRepository, this.xmlFactory);
            this.injectedList = FerricHelper.ConvertToCharacterList("Hello World. This is a great test.");
        }

        [TestCleanup()]
        public virtual void TestCleanup()
        {
            this.ferricNodeUnderTest = null;
            this.ferricNodeRepository = null;
            this.injectedList = null;

            Thread.Sleep(100);
        }

        [TestMethod]
        public void Decrypt_Test_Properly_Decrypts_Encrypted_Data()
        {
            //Arrange
            byte[] actual;
            byte[] expected = FerricHelper.ConvertToByteArray(this.injectedList);
            byte[] injected = FerricHelper.ConvertToByteArray(this.injectedList);

            //Act
            byte[] encrypted = this.ferricNodeUnderTest.Encrypt(injected);

            actual = this.ferricNodeUnderTest.Decrypt(encrypted);

            //Assert
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void Decrypt_Test_Properly_Decrypts_Large_Amount_Of_Encrypted_Data()
        {
            //Arrange
            byte[] actual;
            byte[] expected = FerricHelper.ConvertToByteArray(this.largeAmountOfData);
            byte[] injected = FerricHelper.ConvertToByteArray(this.largeAmountOfData);

            byte[] encrypted = this.ferricNodeUnderTest.Encrypt(injected);

            //Act
            actual = this.ferricNodeUnderTest.Decrypt(encrypted);

            //Assert
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        protected void CompareByteIndexes(IByteIndex expected, IByteIndex actual)
        {
            Assert.AreEqual(expected.ID, actual.ID);
            Assert.AreEqual(expected.Count, actual.Count);

            for (int i = 0; i < expected.Data.Count; i++)
                Assert.AreEqual(expected.Data[i], actual.Data[i]);
        }

        protected void CompareByteMaps(IByteMap expected, IByteMap actual)
        {
            Assert.AreEqual(expected.ID, actual.ID);
            Assert.AreEqual(expected.Count, actual.Count);

            foreach (byte key in expected.Data.Keys)
            {
                Assert.IsTrue(actual.Data.ContainsKey(key));
                Assert.AreEqual(expected.Data[key], actual.Data[key]);
            }
        }

        protected void CompareCaesarShifts(CaesarShift.CaesarShift expected, CaesarShift.CaesarShift actual)
        {
                Assert.AreEqual(expected.Amount, actual.Amount);
                Assert.AreEqual(expected.ShiftDirection, actual.ShiftDirection);
        }

        protected void CompareCiphers(ICipher expected, ICipher actual)
        {
            if (expected is IRandomDataInserts)
            {
                Assert.IsTrue(actual is IRandomDataInserts);

                CompareRandomDataInserts((IRandomDataInserts)expected, (IRandomDataInserts)actual);
            }
            else
            {
                CompareCipherTypes(expected.CipherType, actual.CipherType);
            }

            Assert.AreEqual(expected.EncryptedLength, actual.EncryptedLength);
            Assert.AreEqual(expected.Length, actual.Length);
        }

        protected void CompareCipherTypes(ICipherType expected, ICipherType actual)
        {
            if (expected is CaesarShift.CaesarShift)
            {
                Assert.IsTrue(actual is CaesarShift.CaesarShift);

                CompareCaesarShifts((CaesarShift.CaesarShift)expected, (CaesarShift.CaesarShift)actual);
            }
            else if (expected is ReplacementMap)
            {
                Assert.IsTrue(actual is ReplacementMap);

                CompareReplacementMaps((ReplacementMap)expected, (ReplacementMap)actual);
            }
            else
            {
                throw new InvalidOperationException("Unable to determine how to test the Cipher Types.");
            }
        }

        protected void CompareDistributedCiphers(IDistributedCipher expected, IDistributedCipher actual)
        {
            Assert.AreEqual(expected.Ciphers.Count, actual.Ciphers.Count);

            for (int i = 0; i < expected.Ciphers.Count; i++)
                CompareCiphers(expected.Ciphers[i], actual.Ciphers[i]);

            CompareHeaders(expected.Header, actual.Header);
        }

        private void CompareFerricNodes(IFerricNode expected, IFerricNode actual)
        {
            if (expected is IDistributedCipher)
            {
                Assert.IsTrue(actual is IDistributedCipher);
                CompareDistributedCiphers((IDistributedCipher)expected, (IDistributedCipher)actual);
            }
            else if (expected is IScrambler)
            {
                Assert.IsTrue(actual is IScrambler);
                CompareScramblers((IScrambler)expected, (IScrambler)actual);
            }
            else
            {
                throw new InvalidOperationException("Unable to determine how to test the Ferric Nodes.");
            }

            if (expected.Child == null)
                Assert.IsNull(actual.Child);
            else
                CompareFerricNodes(expected.Child, actual.Child);
        }

        protected void CompareHeaders(IHeader expected, IHeader actual)
        {
            if (expected == null)
            {
                Assert.IsNull(actual);
            }
            else
            {
                CompareByteIndexes(expected.ByteIndex, actual.ByteIndex);
                Assert.AreEqual(expected.EncryptedLength, actual.EncryptedLength);
                Assert.AreEqual(expected.Key, actual.Key);
            }
        }

        protected void CompareRandomDataInserts(IRandomDataInserts expected, IRandomDataInserts actual)
        {
            CompareByteIndexes(expected.ByteIndex, actual.ByteIndex);

            try { Assert.IsNull(actual.CipherType); Assert.Fail(); } catch { }

            Assert.AreEqual(expected.EncryptedLength, actual.EncryptedLength);
            Assert.AreEqual(expected.Length, actual.Length);
        }

        protected void CompareReplacementMaps(ReplacementMap expected, ReplacementMap actual)
        {
            CompareByteMaps(expected.ByteMap, actual.ByteMap);
        }

        protected void CompareScramblers(IScrambler expected, IScrambler actual)
        {
            Assert.AreEqual(expected.ScrambledIndexes.Count, actual.ScrambledIndexes.Count);

            for (int i = 0; i < expected.ScrambledIndexes.Count; i++)
                Assert.AreEqual(expected.ScrambledIndexes[i], actual.ScrambledIndexes[i]);
        }
    }
}
