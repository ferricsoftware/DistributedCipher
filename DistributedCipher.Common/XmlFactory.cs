using System;
using System.Xml;

using DistributedCipher.Framework;

namespace DistributedCipher.Common
{
    public class XmlFactory : IXmlFactory
    {
        public XmlDocument GenerateXmlDocument()
        {
            return new XmlDocument();
        }
    }
}
