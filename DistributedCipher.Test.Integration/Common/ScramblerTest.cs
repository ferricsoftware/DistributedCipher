using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using DistributedCipher.ByteSetRepository.Xml;
using DistributedCipher.Common;
using DistributedCipher.Framework;

namespace DistributedCipher.Test.Integration.Common
{
    [TestClass]
    public class ScramblerTest : IScramblerTest
    {
        [TestInitialize]
        public override void TestInitialize()
        {
            IByteSetFactory byteSetFactory = new ByteSetFactory();
            string fileName = "ScramblerTest.xml";
            Guid id = Guid.NewGuid();
            IXmlFactory xmlFactory = new XmlFactory();

            this.byteSetRepository = new ByteSetXmlRepository(fileName, byteSetFactory, xmlFactory);
            this.xmlFactory = xmlFactory;

            base.TestInitialize();

            this.ferricNodeUnderTest = new Scrambler(id, this.scrambledIndexes);
        }
    }
}
