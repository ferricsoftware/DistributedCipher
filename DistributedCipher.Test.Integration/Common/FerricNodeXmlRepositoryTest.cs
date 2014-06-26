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
    public class FerricNodeXmlRepositoryTest : IFerricNodeRepositoryTest
    {
        protected IByteSetRepository byteSetRepository;
        protected string fileName;
        protected string path;
        protected IXmlFactory xmlFactory;

        [TestInitialize]
        public override void TestInitialize()
        {
            IByteSetFactory byteSetFactory = new ByteSetFactory();
            string byteSetRepositoryFileName = @"FerricNodeXmlRepositoryTestSupplement.xml";
            IXmlFactory xmlFactory = new XmlFactory();
            Random random = new Random();

            this.fileName = @"FerricNodeXmlRepositoryTest.xml";
            this.path = @"XML";
            this.xmlFactory = xmlFactory;

            this.byteSetRepository = new ByteSetXmlRepository(path, byteSetRepositoryFileName, byteSetFactory, xmlFactory);

            FileInfo xmlFile = new FileInfo(Path.Combine(this.path, this.fileName));
            if (!xmlFile.Directory.Exists)
                xmlFile.Directory.Create();
            if (xmlFile.Exists)
                xmlFile.Delete();

            this.ferricNode1 = new Scrambler(Guid.NewGuid(), new List<int>() { 0, 1, 0, 2, 4, 0, 3 });

            IHeader header2 = new Header(new ByteIndex(new List<byte>() { Convert.ToByte('a'), Convert.ToByte('b') }), 2);
            Dictionary<byte, byte> dictionary2 = new Dictionary<byte, byte>();
            dictionary2[Convert.ToByte('a')] = Convert.ToByte('b');
            dictionary2[Convert.ToByte('b')] = Convert.ToByte('a');
            ICipher cipher2 = new Cipher(new ReplacementMap(new ByteMap(dictionary2)));
            this.ferricNode2 = new DistributedCipher.Common.DistributedCipher(Guid.NewGuid(), this.byteSetRepository, header2, new List<ICipher>() { cipher2 }, random);

            IHeader header3 = new Header(new ByteIndex(new List<byte>() { Convert.ToByte('c'), Convert.ToByte('d') }), 5);
            Dictionary<byte, byte> dictionary3 = new Dictionary<byte, byte>();
            dictionary3[Convert.ToByte('c')] = Convert.ToByte('d');
            dictionary3[Convert.ToByte('d')] = Convert.ToByte('c');
            ICipher cipher3 = new Cipher(new ReplacementMap(new ByteMap(dictionary3)));
            this.ferricNode3 = new DistributedCipher.Common.DistributedCipher(Guid.NewGuid(), this.byteSetRepository, header3, new List<ICipher>() { cipher3 }, this.ferricNode4, random);

            this.ferricNode4 = new Scrambler(Guid.NewGuid(), new List<int>() { 0, 1, 0, -1, 2, -2, 3 }, this.ferricNode2);

            this.ferricNodeRepositoryUnderTest = new FerricNodeXmlRepository(this.path, this.fileName, this.byteSetRepository, this.xmlFactory);

            base.TestInitialize();
        }
    }
}
