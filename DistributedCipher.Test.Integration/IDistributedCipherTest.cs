using System;
using System.Collections.Generic;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using DistributedCipher.Common;
using DistributedCipher.Framework;

namespace DistributedCipher.Test.Integration
{
    public abstract class IDistributedCipherTest : IFerricNodeTest
    {
        protected IList<ICipher> ciphers;
        protected IDistributedCipher distributedCipherUnderTest;
        protected IHeader header;
        protected Guid id;

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();

            this.largeAmountOfData = new List<byte>();
            for (int i = Convert.ToInt32(byte.MinValue); i < Convert.ToInt32(byte.MaxValue); i++)
                for (int n = 0; n < this.ciphers.Count + 2; n++)
                    this.largeAmountOfData.Add(Convert.ToByte(i));
        }

        [TestCleanup()]
        public override void TestCleanup()
        {
            base.TestCleanup();

            this.ciphers = null;
            this.distributedCipherUnderTest = null;
            this.header = null;
        }
    }
}
