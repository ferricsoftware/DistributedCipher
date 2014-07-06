using System;
using System.Collections.Generic;

using DistributedCipher.Framework;
using DistributedCipher.Common;

namespace DistributedCipher.Configurator.ViewModels
{
    public class ByteMapViewModel : ByteSetViewModel
    {
        protected IList<byte> keys;

        public IByteMap ByteMap
        {
            get
            {
                Dictionary<byte, byte> dictionary = new Dictionary<byte, byte>();

                for (int i = 0; i < this.bytes.Count; i++)
                {
                    byte _key = this.keys[i];
                    byte _value = this.Bytes[i];

                    dictionary[_key] = _value;
                }

                return new ByteMap(dictionary);
            }
        }

        //public override IList<byte> Bytes 
        //{ 
        //    get { return base; } 
        //    set { base = value; } 
        //}

        public IList<byte> Keys 
        {
            get { return this.keys; } 
            set { this.keys = value; } 
        }
        
        public ByteMapViewModel()
        {
            this.bytes = new List<byte>();
        }
    }
}
