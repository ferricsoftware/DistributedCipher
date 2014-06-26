using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using DistributedCipher.ByteSetRepository.Xml;
using DistributedCipher.Common;
using DistributedCipher.Framework;

namespace DistributedCipher.Test.Integration.Common
{
    [TestClass]
    public class NestedDistributedCipherTest : IDistributedCipherTest
    {
        [TestInitialize]
        public override void TestInitialize()
        {
            IByteSetFactory byteSetFactory = new ByteSetFactory();
            Dictionary<byte, byte> data;
            string fileName = @"NestedDistributedCipherTest.xml";
            IXmlFactory xmlFactory = new XmlFactory();

            data = new Dictionary<byte, byte>();
            data[Convert.ToByte('H')] = Convert.ToByte('h');
            data[Convert.ToByte('e')] = Convert.ToByte('H');
            data[Convert.ToByte('h')] = Convert.ToByte('e');
            IByteMap firstByteMap = new ByteMap(data);

            data = new Dictionary<byte, byte>();
            data[Convert.ToByte('W')] = Convert.ToByte('e');
            data[Convert.ToByte('a')] = Convert.ToByte('W');
            data[Convert.ToByte('e')] = Convert.ToByte('a');
            IByteMap secondByteMap = new ByteMap(data);

            ReplacementMap firstReplacementMap = new ReplacementMap(firstByteMap);
            ReplacementMap secondReplacementMap = new ReplacementMap(secondByteMap);

            this.byteSetRepository = new ByteSetXmlRepository(fileName, byteSetFactory, xmlFactory);
            this.ciphers = new List<ICipher>();
            this.ciphers.Add(new Cipher(firstReplacementMap));
            this.header = null;
            this.id = Guid.NewGuid();
            this.xmlFactory = xmlFactory;

            base.TestInitialize();

            IFerricNode child = new DistributedCipher.Common.DistributedCipher(Guid.NewGuid(), this.byteSetRepository, this.header, new List<ICipher>() { new Cipher(secondReplacementMap) }, FerricHelper.Random);

            this.ferricNodeUnderTest = new DistributedCipher.Common.DistributedCipher(this.id, this.byteSetRepository, this.header, this.ciphers, child, FerricHelper.Random);
        }
    }
}
