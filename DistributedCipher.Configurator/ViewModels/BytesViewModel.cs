using System;
using System.Collections.Generic;

using DistributedCipher.Framework;

namespace DistributedCipher.Configurator.ViewModels
{
    public class BytesViewModel
    {
        protected IList<IList<byte>> bytes ;

        public IList<IList<byte>> Bytes { get { return this.bytes; } set { this.bytes = value; } }

        public BytesViewModel(IList<IList<byte>> bytes)
        {
            this.bytes = bytes;

            //this.bytes = new List<IList<byte>>();
            //this.bytes.Add(DistributedCipherHelper.AlphaNumeric);
            //this.bytes.Add(DistributedCipherHelper.KeyboardCharacters);
            //this.bytes.Add(DistributedCipherHelper.Letters);
            //this.bytes.Add(DistributedCipherHelper.LowerCaseLetters);
            //this.bytes.Add(DistributedCipherHelper.Numbers);
            //this.bytes.Add(DistributedCipherHelper.UpperCaseLetters);
        }
    }
}
