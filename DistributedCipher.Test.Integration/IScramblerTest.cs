using System;
using System.Collections.Generic;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using DistributedCipher.Common;
using DistributedCipher.Framework;

namespace DistributedCipher.Test.Integration
{
    public abstract class IScramblerTest : IFerricNodeTest
    {
        protected IList<int> scrambledIndexes;
        protected IScrambler scramblerUnderTest;

        [TestInitialize()]
        public override void TestInitialize()
        {
            base.TestInitialize();

            this.scrambledIndexes = new List<int>() {
                0, -1, 1, 3, -2, 4, 3, 2, -0, -7, 8, -10, 3, 6, 10, -14, 8, 14, 0, -3, -17
            };

            this.largeAmountOfData = new List<byte>();
            for (int i = Convert.ToInt32(byte.MinValue); i < Convert.ToInt32(byte.MaxValue); i++)
                for (int n = 0; n < this.scrambledIndexes.Count + 2; n++)
                    this.largeAmountOfData.Add(Convert.ToByte(i));
        }

        [TestCleanup()]
        public override void TestCleanup()
        {
            base.TestCleanup();

            this.scrambledIndexes = null;
            this.scramblerUnderTest = null;
        }
    }
}
