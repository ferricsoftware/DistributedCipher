using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using DistributedCipher.ByteSetRepository.Xml;
using DistributedCipher.Common;
using DistributedCipher.Framework;

namespace DistributedCipher.Test.Integration.Common
{
    [TestClass]
    public class NestedScramblerTest : IScramblerTest
    {
        [TestInitialize]
        public override void TestInitialize()
        {
            IByteSetFactory byteSetFactory = new ByteSetFactory();
            string fileName = "NestedScramblerTest.xml"; 
            IXmlFactory xmlFactory = new XmlFactory();

            this.byteSetRepository = new ByteSetXmlRepository(fileName, byteSetFactory, xmlFactory);
            this.xmlFactory = xmlFactory;

            base.TestInitialize();

            IFerricNode child = new Scrambler(Guid.NewGuid(), new List<int>() { 0, 1, 2, 3, 4 });

            this.ferricNodeUnderTest = new Scrambler(Guid.NewGuid(), this.scrambledIndexes, child);
        }
    }
}
