using System;
using System.Runtime.Serialization;

namespace DistributedCipher.Framework
{
    public class InvalidMappedKeyException : Exception
    {
        public InvalidMappedKeyException() : base() { }
        public InvalidMappedKeyException(string message) : base(message) { }
        public InvalidMappedKeyException(string message, Exception innerException) : base(message, innerException) { }
        public InvalidMappedKeyException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
