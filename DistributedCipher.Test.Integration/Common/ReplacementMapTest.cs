using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using DistributedCipher.ByteSetRepository.Xml;
using DistributedCipher.Common;
using DistributedCipher.Framework;

namespace DistributedCipher.Test.Integration.Common
{
    [TestClass]
    public class ReplacementMapTest : IDistributedCipherTest
    {
        [TestInitialize]
        public override void TestInitialize()
        {
            IByteSetFactory byteSetFactory = new ByteSetFactory();
            Dictionary<byte, byte> data;
            string fileName = @"ReplacementMapTest.xml";
            IXmlFactory xmlFactory = new XmlFactory();

            data = new Dictionary<byte, byte>();
            data[Convert.ToByte('H')] = Convert.ToByte('h');
            data[Convert.ToByte('e')] = Convert.ToByte('i');
            data[Convert.ToByte('h')] = Convert.ToByte(' ');
            data[Convert.ToByte('i')] = Convert.ToByte('o');
            data[Convert.ToByte('l')] = Convert.ToByte('H');
            data[Convert.ToByte('o')] = Convert.ToByte('l');
            data[Convert.ToByte('s')] = Convert.ToByte('.');
            data[Convert.ToByte('t')] = Convert.ToByte('e');
            data[Convert.ToByte(' ')] = Convert.ToByte('s');
            data[Convert.ToByte('.')] = Convert.ToByte('t');
            IByteMap firstByteMap = new ByteMap(data);

            data = new Dictionary<byte, byte>();
            data[Convert.ToByte('W')] = Convert.ToByte('e');
            data[Convert.ToByte('a')] = Convert.ToByte('s');
            data[Convert.ToByte('e')] = Convert.ToByte('.');
            data[Convert.ToByte('s')] = Convert.ToByte('a');
            data[Convert.ToByte(' ')] = Convert.ToByte('W');
            data[Convert.ToByte('.')] = Convert.ToByte(' ');
            IByteMap secondByteMap = new ByteMap(data);

            data = new Dictionary<byte, byte>();
            data[Convert.ToByte('s')] = Convert.ToByte('t');
            data[Convert.ToByte('r')] = Convert.ToByte('s');
            data[Convert.ToByte('t')] = Convert.ToByte('r');
            IByteMap thirdByteMap = new ByteMap(data);

            ReplacementMap firstReplacementMap = new ReplacementMap(firstByteMap);
            ReplacementMap secondReplacementMap = new ReplacementMap(secondByteMap);
            ReplacementMap thirdReplacementMap = new ReplacementMap(thirdByteMap);

            this.byteSetRepository = new ByteSetXmlRepository(fileName, byteSetFactory, xmlFactory);
            this.ciphers = new List<ICipher>();
            this.ciphers.Add(new Cipher(firstReplacementMap));
            this.ciphers.Add(new Cipher(secondReplacementMap));
            this.ciphers.Add(new Cipher(firstReplacementMap, 2));
            this.ciphers.Add(new Cipher(thirdReplacementMap));
            this.header = null;
            this.id = Guid.NewGuid();
            this.xmlFactory = xmlFactory;

            base.TestInitialize();

            this.ferricNodeUnderTest = new DistributedCipher.Common.DistributedCipher(this.id, this.byteSetRepository, this.header, this.ciphers, FerricHelper.Random);
        }
    }
}
