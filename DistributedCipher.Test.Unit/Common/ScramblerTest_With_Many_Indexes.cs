using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using DistributedCipher.Common;

namespace DistributedCipher.Test.Unit.Common
{
    [TestClass]
    public class ScramblerTest_With_Many_Indexes : IScramblerTest
    {
        [TestInitialize()]
        public override void TestInitialize()
        {
            Guid id = Guid.NewGuid();

            base.TestInitialize();

            this.scrambledIndexes = new List<int>() { 0, 0, 1, 1, 3, 0, 6, 4, 3, 2, 9 };

            this.scramblerUnderTest = new Scrambler(id, this.scrambledIndexes);
        }
    }
}
