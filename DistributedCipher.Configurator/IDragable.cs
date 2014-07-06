using System;

namespace DistributedCipher.Configurator
{
    public interface IDragable
    {
        Type DataType { get; }

        void Remove(object o);
    }
}
