using System;
using System.Collections.Generic;

using DistributedCipher.Framework;
using DistributedCipher.Common;
using DistributedCipher.Configurator.Models;

namespace DistributedCipher.Configurator.ViewModels
{
    public abstract class ByteSetViewModel
    {
        protected IList<byte> bytes;
        protected IList<NameModel> names;
        protected IList<byte> remainingBytes;

        public virtual IList<byte> Bytes { get { return this.bytes; } set { this.bytes = value; UpdateRemainingBytes(); } }
        public IList<NameModel> Names { get { return this.names; } set { this.names = value; } }
        public IList<byte> RemainingBytes { get { return this.remainingBytes; } }

        public ByteSetViewModel()
        {
            this.bytes = new List<byte>();
            this.names = new List<NameModel>();

            UpdateRemainingBytes();
        }

        protected void UpdateRemainingBytes()
        {
            this.remainingBytes = new List<byte>();

            for (int i = (int)byte.MinValue; i <= (int)byte.MaxValue; i++)
                if (!this.bytes.Contains((byte)i))
                    this.remainingBytes.Add((byte)i);
        }
    }
}
