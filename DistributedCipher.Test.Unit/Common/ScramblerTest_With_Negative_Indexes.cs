using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using DistributedCipher.Common;

namespace DistributedCipher.Test.Unit.Common
{
    [TestClass]
    public class ScramblerTest_With_Negative_Indexes : IScramblerTest
    {
        [TestInitialize()]
        public override void TestInitialize()
        {
            Guid id = Guid.NewGuid();

            base.TestInitialize();

            this.scrambledIndexes = new List<int>() { 0, 1, -1 };

            this.scramblerUnderTest = new Scrambler(id, this.scrambledIndexes);
        }
    }
}
