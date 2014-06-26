using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using DistributedCipher.Common;
using DistributedCipher.Framework;
using DistributedCipher.ByteSetRepository.Xml;

namespace DistributedCipher.Test.Integration.Common
{
    [TestClass]
    public class HeaderTest : IDistributedCipherTest
    {
        [TestInitialize]
        public override void TestInitialize()
        {
            IByteSetFactory byteSetFactory = new ByteSetFactory();
            ICipherType dummy = new ReplacementMap(new ByteMap(new Dictionary<byte, byte>()));
            string fileName = @"HeaderTest.xml";
            IXmlFactory xmlFactory = new XmlFactory();

            this.byteSetRepository = new ByteSetXmlRepository(fileName, byteSetFactory, xmlFactory);
            this.ciphers = new List<ICipher>();
            this.ciphers.Add(new Cipher(dummy));

            IByteIndex byteIndex = new ByteIndex(FerricHelper.Numbers);
            this.header = new Header(byteIndex, "00100");

            this.xmlFactory = xmlFactory;

            base.TestInitialize();

            this.ferricNodeUnderTest = new DistributedCipher.Common.DistributedCipher(Guid.NewGuid(), byteSetRepository, this.header, this.ciphers, FerricHelper.Random);
        }
    }
}
