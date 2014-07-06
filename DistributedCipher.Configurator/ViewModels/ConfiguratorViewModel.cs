using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using DistributedCipher.Common;
using DistributedCipher.Framework;

namespace DistributedCipher.Configurator.ViewModels
{
    public class ConfiguratorViewModel
    {
        protected ByteIndexViewModel byteIndexViewModel;
        protected ObservableCollection<ICipher> ciphers;
        protected string message;

        public ByteIndexViewModel ByteIndexViewModel { get { return this.byteIndexViewModel; } }
        public ObservableCollection<ICipher> Ciphers { get { return this.ciphers; } }
        public string Message { get { return this.message; } set { this.message = value; } }

        public ConfiguratorViewModel()
        {
            this.byteIndexViewModel = new ByteIndexViewModel();
            this.ciphers = new ObservableCollection<ICipher>();
            this.ciphers.Add(new Cipher(new ReplacementMap(new ByteMap(new List<byte>())), 5));
            this.ciphers.Add(new Cipher(new ReplacementMap(new ByteMap(new List<byte>())), 2));
        }
    }
}
