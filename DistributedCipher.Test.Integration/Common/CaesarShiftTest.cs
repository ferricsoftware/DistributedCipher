using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using DistributedCipher.ByteSetRepository.Xml;
using DistributedCipher.CaesarShift;
using DistributedCipher.Common;
using DistributedCipher.Framework;

namespace DistributedCipher.Test.Integration.Common
{
    [TestClass]
    public class CaesarShiftTest : IDistributedCipherTest
    {
        [TestInitialize]
        public override void TestInitialize()
        {
            IByteSetFactory byteSetFactory = new ByteSetFactory();
            string fileName = @"CaesarShiftTest.xml";
            IXmlFactory xmlFactory = new XmlFactory();

            this.byteSetRepository = new ByteSetXmlRepository(fileName, byteSetFactory, xmlFactory);
            this.ciphers = new List<ICipher>();
            this.ciphers.Add(new Cipher(new CaesarShift.CaesarShift()));
            this.ciphers.Add(new Cipher(new CaesarShift.CaesarShift(7, ShiftDirection.Left)));
            this.ciphers.Add(new Cipher(new CaesarShift.CaesarShift(1, ShiftDirection.Left)));
            this.ciphers.Add(new Cipher(new CaesarShift.CaesarShift(6)));
            this.ciphers.Add(new Cipher(new CaesarShift.CaesarShift(9)));
            this.header = null;
            this.id = Guid.NewGuid();
            this.xmlFactory = xmlFactory;

            base.TestInitialize();

            this.ferricNodeUnderTest = new DistributedCipher.Common.DistributedCipher(this.id, this.byteSetRepository, this.header, this.ciphers, FerricHelper.Random);
        }
    }
}
