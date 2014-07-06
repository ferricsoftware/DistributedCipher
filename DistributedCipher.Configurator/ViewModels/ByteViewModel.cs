using System;

namespace DistributedCipher.Configurator.ViewModels
{
    public class ByteViewModel : IDragable
    {
        protected byte _byte;

        public byte Byte { get { return this._byte; } }
        public Type DataType { get { return typeof(ByteViewModel); } }

        public ByteViewModel(byte _byte)
        {
            this._byte = _byte;
        }

        public void Remove(object o)
        {
        }
    }
}
