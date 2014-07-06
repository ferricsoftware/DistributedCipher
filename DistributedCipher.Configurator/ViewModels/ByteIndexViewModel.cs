using System;
using System.Collections.Generic;

using DistributedCipher.Framework;
using DistributedCipher.Common;

namespace DistributedCipher.Configurator.ViewModels
{
    public class ByteIndexViewModel : ByteSetViewModel
    {
        public IByteIndex ByteSet { get { return new ByteIndex(this.bytes); } }

        public ByteIndexViewModel()
        {
        }
    }
}
