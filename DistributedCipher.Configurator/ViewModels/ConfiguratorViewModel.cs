using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using DistributedCipher.Common;
using DistributedCipher.Framework;

namespace DistributedCipher.Configurator.ViewModels
{
    public class ConfiguratorViewModel
    {
        protected ObservableCollection<ICipher> ciphers;

        public ObservableCollection<ICipher> Ciphers { get { return this.ciphers; } }

        public ConfiguratorViewModel()
        {
            this.ciphers = new ObservableCollection<ICipher>();
            this.ciphers.Add(new Cipher(new ReplacementMap(new ByteMap(new List<byte>()))));
        }
    }
}
