using System;
using System.Xml;

namespace DistributedCipher.Framework
{
    public interface IXmlFactory
    {
        XmlDocument GenerateXmlDocument();
    }
}
