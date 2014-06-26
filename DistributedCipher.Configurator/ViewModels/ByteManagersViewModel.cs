using System;
using System.Collections.Generic;

using DistributedCipher.Framework;

namespace DistributedCipher.Configurator.ViewModels
{
    public class ByteManagersViewModel
    {
        protected IList<IByteSet> byteSets = new List<IByteSet>();

        public IList<IByteSet> ByteManagers { get { return this.byteSets; } set { this.byteSets = value; } }

        public ByteManagersViewModel(IList<IByteSet> byteSets)
        {
            this.byteSets = byteSets;
        }
    }
}
