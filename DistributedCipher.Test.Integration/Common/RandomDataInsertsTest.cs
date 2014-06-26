using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using DistributedCipher.ByteSetRepository.Xml;
using DistributedCipher.Common;
using DistributedCipher.Framework;

namespace DistributedCipher.Test.Integration.Common
{
    [TestClass]
    public class RandomDataInsertsTest : IDistributedCipherTest
    {
        [TestInitialize]
        public override void TestInitialize()
        {
            IByteSetFactory byteSetFactory = new ByteSetFactory();
            ICipherType dummy = new ReplacementMap(new ByteMap(new Dictionary<byte, byte>()));
            string fileName = @"RandomDataInsertsTest.xml";
            IXmlFactory xmlFactory = new XmlFactory();

            this.byteSetRepository = new ByteSetXmlRepository(fileName, byteSetFactory, xmlFactory);
            this.ciphers = new List<ICipher>();
            this.ciphers.Add(new Cipher(dummy));
            this.ciphers.Add(new RandomDataInserts(new ByteIndex(FerricHelper.AlphaNumeric)));
            this.ciphers.Add(new RandomDataInserts(new ByteIndex(FerricHelper.KeyboardCharacters)));
            this.ciphers.Add(new Cipher(dummy));
            this.ciphers.Add(new RandomDataInserts(new ByteIndex(FerricHelper.Numbers)));
            this.ciphers.Add(new Cipher(dummy));
            this.ciphers.Add(new Cipher(dummy));
            this.header = null;
            this.id = Guid.NewGuid();
            this.xmlFactory = xmlFactory;

            base.TestInitialize();

            this.ferricNodeUnderTest = new DistributedCipher.Common.DistributedCipher(this.id, this.byteSetRepository, this.header, this.ciphers, FerricHelper.Random);
        }
    }
}
