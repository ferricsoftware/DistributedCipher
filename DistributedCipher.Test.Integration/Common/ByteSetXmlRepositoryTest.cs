using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using DistributedCipher.ByteSetRepository.Xml;
using DistributedCipher.Common;
using DistributedCipher.Framework;
using DistributedCipher.Test.Unit;

namespace DistributedCipher.Test.Integration.Common
{
    [TestClass]
    public class ByteSetXmlRepositoryTest : IByteSetRepositoryTest
    {
        protected IByteSetFactory byteSetFactory;
        protected string fileName;
        protected string path;
        protected IXmlFactory xmlFactory;

        [TestInitialize]
        public override void TestInitialize()
        {
            this.byteSetFactory = new ByteSetFactory();
            this.fileName = @"ByteSetXmlRepositoryTest.xml";
            this.path = @"XML";
            this.xmlFactory = new XmlFactory();

            FileInfo xmlFile = new FileInfo(Path.Combine(this.path, this.fileName));
            if(!xmlFile.Directory.Exists)
                xmlFile.Directory.Create();
            if(xmlFile.Exists)
                xmlFile.Delete();

            this.byteSet1 = new ByteIndex(new byte[] { Convert.ToByte('a'), Convert.ToByte('b'), Convert.ToByte('c') });
            this.byteSet2 = new ByteIndex(new byte[] { Convert.ToByte('a'), Convert.ToByte('b'), Convert.ToByte('d') });

            Dictionary<byte, byte> data = new Dictionary<byte, byte>();
            data[Convert.ToByte('a')] = Convert.ToByte('b');
            data[Convert.ToByte('b')] = Convert.ToByte('c');
            data[Convert.ToByte('c')] = Convert.ToByte('a');
            this.byteSet3 = new ByteMap(data);

            this.byteSetRepositoryUnderTest = new ByteSetXmlRepository(this.path, this.fileName, this.byteSetFactory, this.xmlFactory);

            base.TestInitialize();
        }
    }
}
