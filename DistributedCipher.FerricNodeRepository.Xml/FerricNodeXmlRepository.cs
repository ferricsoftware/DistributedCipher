using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.IO;
using System.Xml;

using DistributedCipher.CaesarShift;
using DistributedCipher.Framework;

namespace DistributedCipher.Common
{
    public class FerricNodeXmlRepository : IFerricNodeRepository
    {
        protected IByteSetRepository byteSetRepository;
        protected string path;
        protected string fullFileName;
        protected string xmlFileName;
        protected IXmlFactory xmlFactory;

        public FerricNodeXmlRepository(string xmlFileName, IByteSetRepository byteSetRepository, IXmlFactory xmlFactory)
            : this(Environment.CurrentDirectory, xmlFileName, byteSetRepository, xmlFactory)
        {
        }

        public FerricNodeXmlRepository(string path, string xmlFileName, IByteSetRepository byteSetRepository, IXmlFactory xmlFactory)
        {
            this.byteSetRepository = byteSetRepository;
            this.path = path;
            this.xmlFileName = xmlFileName;
            this.xmlFactory = xmlFactory;

            if (!this.xmlFileName.EndsWith(".xml"))
            {
                if (!this.xmlFileName.EndsWith("."))
                    this.xmlFileName += ".";

                this.xmlFileName += "xml";
            }

            if (this.path == null)
                this.fullFileName = this.xmlFileName;
            else
                this.fullFileName = Path.Combine(this.path, this.xmlFileName);
        }

        public void Delete(IFerricNode ferricNode)
        {
            if (ferricNode == null)
                throw new ArgumentNullException("Ferric Node");

            Dictionary<Guid, IFerricNode> ferricNodes = LoadFromFile();

            if (!ferricNodes.ContainsKey(ferricNode.ID))
                throw new KeyNotFoundException(ferricNode.ID.ToString());

            ferricNodes.Remove(ferricNode.ID);

            SaveToFile(ferricNodes);
        }

        public Guid Exists(IFerricNode ferricNode)
        {
            if (ferricNode == null)
                throw new ArgumentNullException("Ferric Node");

            Dictionary<Guid, IFerricNode> ferricNodes = LoadFromFile();

            return Exists(ferricNodes, ferricNode);
        }

        public IFerricNode Find(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID is empty.");

            Dictionary<Guid, IFerricNode> ferricNodes = LoadFromFile();

            if (ferricNodes.ContainsKey(id))
                return ferricNodes[id];

            return null;
        }

        public void Save(IFerricNode ferricNode)
        {
            if (ferricNode == null)
                throw new ArgumentNullException("Byte Set");

            Dictionary<Guid, IFerricNode> ferricNodes = LoadFromFile();

            if (Exists(ferricNodes, ferricNode) != Guid.Empty)
                throw new DuplicateKeyException(ferricNode.ID);

            ferricNodes[ferricNode.ID] = ferricNode;

            SaveToFile(ferricNodes);
        }

        protected Guid Exists(Dictionary<Guid, IFerricNode> ferricNodes, IFerricNode ferricNode)
        {
            if (ferricNodes.ContainsKey(ferricNode.ID))
                return ferricNode.ID;

            return Guid.Empty;
        }

        protected Dictionary<Guid, IFerricNode> LoadFromFile()
        {
            FileInfo xmlFile = new FileInfo(this.fullFileName);
            if (!xmlFile.Exists)
                return new Dictionary<Guid, IFerricNode>();

            XmlDocument xmlDocument = this.xmlFactory.GenerateXmlDocument();
            xmlDocument.Load(this.fullFileName);

            return GenerateFerricNodes(xmlDocument.DocumentElement);
        }

        protected void SaveToFile(Dictionary<Guid, IFerricNode> ferricNodes)
        {
            FileInfo xmlFile = new FileInfo(this.fullFileName);
            if (!xmlFile.Directory.Exists)
                xmlFile.Directory.Create();

            XmlDocument xmlDocument = this.xmlFactory.GenerateXmlDocument();
            xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", null, null));
            xmlDocument.AppendChild(CreateFerricNodesElement(ferricNodes, xmlDocument));

            xmlDocument.Save(Path.Combine(this.fullFileName));
        }

        #region XML Element Creation Functions

        protected XmlAttribute CreateAttribute(XmlDocument xmlDocument, string name, string value)
        {
            XmlAttribute attribute = xmlDocument.CreateAttribute(name);

            attribute.Value = value;

            return attribute;
        }

        protected XmlElement CreateByteIndexReferenceElement(IByteIndex byteIndex, XmlDocument xmlDocument)
        {
            XmlElement byteIndexReferenceElement = xmlDocument.CreateElement("IByteIndex");

            byteIndexReferenceElement.Attributes.Append(CreateAttribute(xmlDocument, "ID", byteIndex.ID.ToString()));

            return byteIndexReferenceElement;
        }

        protected XmlElement CreateByteMapReferenceElement(IByteMap byteMap, XmlDocument xmlDocument)
        {
            XmlElement byteMapReferenceElement = xmlDocument.CreateElement("IByteMap");

            byteMapReferenceElement.Attributes.Append(CreateAttribute(xmlDocument, "ID", byteMap.ID.ToString()));

            return byteMapReferenceElement;
        }

        protected XmlNode CreateCaesarShiftElement(CaesarShift.CaesarShift caesarShift, XmlDocument xmlDocument)
        {
            XmlElement caesarShiftElement = CreateCipherTypeElement(caesarShift, xmlDocument);

            if (caesarShift.ShiftDirection != ShiftDirection.Right)
            {
                XmlElement shiftDirectionElement = xmlDocument.CreateElement("ShiftDirection");

                shiftDirectionElement.AppendChild(xmlDocument.CreateTextNode(caesarShift.ShiftDirection.ToString()));

                caesarShiftElement.AppendChild(shiftDirectionElement);
            }

            if (caesarShift.Amount != 1)
            {
                XmlElement amountElement = xmlDocument.CreateElement("Amount");

                amountElement.AppendChild(xmlDocument.CreateTextNode(caesarShift.Amount.ToString()));

                caesarShiftElement.AppendChild(amountElement);
            }

            return caesarShiftElement;
        }

        protected XmlElement CreateCipherElement(ICipher cipher, XmlDocument xmlDocument)
        {
            XmlElement cipherElement = xmlDocument.CreateElement("ICipher");

            if (!(cipher is Cipher))
            {
                cipherElement.Attributes.Append(CreateAttribute(xmlDocument, "Assembly", GetAssemblyName(cipher)));
                cipherElement.Attributes.Append(CreateAttribute(xmlDocument, "Type", GetTypeName(cipher)));
            }

            if (cipher.CipherType is CaesarShift.CaesarShift)
                cipherElement.AppendChild(CreateCaesarShiftElement((CaesarShift.CaesarShift)cipher.CipherType, xmlDocument));
            else if (cipher.CipherType is ReplacementMap)
                cipherElement.AppendChild(CreateReplacementMapElement((ReplacementMap)cipher.CipherType, xmlDocument));
            else
                cipherElement.AppendChild(CreateCipherTypeElement(cipher.CipherType, xmlDocument));

            if (cipher.Length > 1)
            {
                XmlElement lengthElement = xmlDocument.CreateElement("Length");
                lengthElement.AppendChild(xmlDocument.CreateTextNode(cipher.Length.ToString()));

                cipherElement.AppendChild(lengthElement);
            }

            return cipherElement;
        }

        protected XmlElement CreateCiphersElement(IList<ICipher> ciphers, XmlDocument xmlDocument)
        {
            XmlElement ciphersElement = xmlDocument.CreateElement("ICiphers");

            foreach (ICipher cipher in ciphers)
            {
                if (cipher is RandomDataInserts)
                    ciphersElement.AppendChild(CreateRandomDataInsertsElement((RandomDataInserts)cipher, xmlDocument));
                else
                    ciphersElement.AppendChild(CreateCipherElement(cipher, xmlDocument));
            }

            return ciphersElement;
        }

        protected XmlElement CreateCipherTypeElement(ICipherType cipherType, XmlDocument xmlDocument)
        {
            XmlElement cipherTypeElement = xmlDocument.CreateElement("ICipherType");

            string assemblyName = GetAssemblyName(cipherType);
            if (assemblyName != GetAssemblyName(this))
                cipherTypeElement.Attributes.Append(CreateAttribute(xmlDocument, "Assembly", assemblyName));

            cipherTypeElement.Attributes.Append(CreateAttribute(xmlDocument, "Type", GetTypeName(cipherType)));

            return cipherTypeElement;
        }

        protected XmlElement CreateDistributedCipherElement(IDistributedCipher distributedCipher, XmlDocument xmlDocument, XmlElement distributedCipherElement)
        {
            if (!(distributedCipher is DistributedCipher))
            {
                distributedCipherElement.Attributes.Append(CreateAttribute(xmlDocument, "Assembly", GetAssemblyName(distributedCipher)));
                distributedCipherElement.Attributes.Append(CreateAttribute(xmlDocument, "Type", GetTypeName(distributedCipher)));
            }

            if (distributedCipher.Header != null)
                distributedCipherElement.AppendChild(CreateHeaderElement(distributedCipher.Header, xmlDocument));

            distributedCipherElement.AppendChild(CreateCiphersElement(distributedCipher.Ciphers, xmlDocument));

            return distributedCipherElement;
        }

        protected XmlNode CreateFerricNodeElement(IFerricNode ferricNode, XmlDocument xmlDocument)
        {
            XmlElement ferricNodeElement = xmlDocument.CreateElement("IFerricNode");

            ferricNodeElement.Attributes.Append(CreateAttribute(xmlDocument, "ID", ferricNode.ID.ToString()));
            if (ferricNode is IDistributedCipher)
            {
                ferricNodeElement.Attributes.Append(CreateAttribute(xmlDocument, "Type", "IDistributedCipher"));

                CreateDistributedCipherElement((IDistributedCipher)ferricNode, xmlDocument, ferricNodeElement);
            }
            else if (ferricNode is IScrambler)
            {
                ferricNodeElement.Attributes.Append(CreateAttribute(xmlDocument, "Type", "IScrambler"));

                CreateScramblerElement((IScrambler)ferricNode, xmlDocument, ferricNodeElement);
            }
            else
            {
                throw new InvalidOperationException("Unsupported Ferric Node provided.");
            }

            if (ferricNode.Child != null)
            {
                XmlElement childElement = xmlDocument.CreateElement("Child");

                childElement.Attributes.Append(CreateAttribute(xmlDocument, "ID", ferricNode.Child.ID.ToString()));

                ferricNodeElement.AppendChild(childElement);
            }

            return ferricNodeElement;
        }

        private XmlNode CreateFerricNodesElement(Dictionary<Guid, IFerricNode> ferricNodes, XmlDocument xmlDocument)
        {
            XmlElement byteSetsElement = xmlDocument.CreateElement("IFerricNodes");

            foreach (Guid id in ferricNodes.Keys)
                byteSetsElement.AppendChild(CreateFerricNodeElement(ferricNodes[id], xmlDocument));

            return byteSetsElement;
        }

        protected XmlElement CreateHeaderElement(IHeader header, XmlDocument xmlDocument)
        {
            XmlElement headerElement = xmlDocument.CreateElement("IHeader");

            if (!(header is Header))
            {
                headerElement.Attributes.Append(CreateAttribute(xmlDocument, "Assembly", GetAssemblyName(header)));
                headerElement.Attributes.Append(CreateAttribute(xmlDocument, "Type", GetTypeName(header)));
            }

            XmlElement keyElement = xmlDocument.CreateElement("Key");
            keyElement.Attributes.Append(CreateAttribute(xmlDocument, "Value", header.Key));

            IByteIndex byteIndex = header.ByteIndex;
            Guid storedID = this.byteSetRepository.Exists(byteIndex);
            if (storedID == Guid.Empty)
                this.byteSetRepository.Save(byteIndex);
            else if (storedID != byteIndex.ID)
                byteIndex = (IByteIndex)this.byteSetRepository.Find(storedID);

            headerElement.AppendChild(keyElement);
            headerElement.AppendChild(CreateByteIndexReferenceElement(byteIndex, xmlDocument));

            return headerElement;
        }

        protected XmlNode CreateIntegerListElement(string elementName, IList<int> integerList, XmlDocument xmlDocument)
        {
            XmlElement integerListElement = xmlDocument.CreateElement(elementName);

            for (int i = 0; i < integerList.Count; i++)
            {
                if (i > 0)
                    integerListElement.InnerText += ", ";

                integerListElement.InnerText += integerList[i].ToString();
            }

            return integerListElement;
        }

        protected XmlNode CreateRandomDataInsertsElement(RandomDataInserts randomDataInserts, XmlDocument xmlDocument)
        {
            XmlElement randomDataInsertsElement = xmlDocument.CreateElement("RandomDataInserts");

            IByteIndex byteIndex = randomDataInserts.ByteIndex;
            Guid storedID = this.byteSetRepository.Exists(byteIndex);
            if (storedID == Guid.Empty)
                this.byteSetRepository.Save(byteIndex);
            else if (storedID != byteIndex.ID)
                byteIndex = (IByteIndex)this.byteSetRepository.Find(storedID);

            randomDataInsertsElement.AppendChild(CreateByteIndexReferenceElement(byteIndex, xmlDocument));

            return randomDataInsertsElement;
        }

        protected XmlNode CreateReplacementMapElement(ReplacementMap replacementMap, XmlDocument xmlDocument)
        {
            XmlElement replacementMapElement = CreateCipherTypeElement(replacementMap, xmlDocument);

            IByteMap byteMap = replacementMap.ByteMap;
            Guid storedID = this.byteSetRepository.Exists(byteMap);
            if (storedID == Guid.Empty)
                this.byteSetRepository.Save(byteMap);
            else if (storedID != byteMap.ID)
                byteMap = (IByteMap)this.byteSetRepository.Find(storedID);

            replacementMapElement.AppendChild(CreateByteMapReferenceElement(byteMap, xmlDocument));

            return replacementMapElement;
        }

        protected XmlNode CreateScramblerElement(IScrambler scrambler, XmlDocument xmlDocument, XmlElement scramblerElement)
        {
            scramblerElement.AppendChild(CreateIntegerListElement("ScrambledIndexes", scrambler.ScrambledIndexes, xmlDocument));

            return scramblerElement;
        }

        #endregion XML Element Creation Functions

        #region XML Element Reading

        protected ICipherType GenerateCaesarShift(XmlElement parentElement)
        {
            ShiftDirection? shiftDirection = null;
            int? amount = null;

            foreach (XmlElement childElement in parentElement)
            {
                if (childElement.Name == "Amount")
                {
                    amount = Convert.ToInt32(childElement.InnerText);
                }
                else if (childElement.Name == "ShiftDirection")
                {
                    if (childElement.InnerText == "Left")
                        shiftDirection = ShiftDirection.Left;
                    else
                        shiftDirection = ShiftDirection.Right;
                }
            }

            if (amount == null)
                amount = 1;

            if (shiftDirection == null)
                return new CaesarShift.CaesarShift(amount.Value);

            return new CaesarShift.CaesarShift(amount.Value, shiftDirection.Value);
        }

        protected ICipher GenerateCipher(XmlElement parentElement)
        {
            ICipherType cipherType = null;
            int length = 1;

            foreach (XmlElement childElement in parentElement)
            {
                if (childElement.Name == "Length")
                {
                    length = Convert.ToInt32(childElement.InnerText);

                    break;
                }
            }

            foreach (XmlElement childElement in parentElement)
                if (childElement.Name == "ICipherType")
                    cipherType = GenerateCipherType(childElement);

            return new Cipher(cipherType, length);
        }

        protected IList<ICipher> GenerateCipherList(XmlElement parentElement)
        {
            IList<ICipher> ciphers = new List<ICipher>();

            foreach (XmlElement childElement in parentElement)
            {
                if (childElement.Name == "ICipher")
                    ciphers.Add(GenerateCipher(childElement));
                else if (childElement.Name == "RandomDataInserts")
                    ciphers.Add(GenerateRandomDataInserts(childElement));
            }

            return ciphers;
        }

        protected ICipherType GenerateCipherType(XmlElement parentElement)
        {
            foreach (XmlAttribute xmlAttribute in parentElement.Attributes)
            {
                if (xmlAttribute.Name == "Type")
                {
                    if (xmlAttribute.Value == "CaesarShift")
                        return GenerateCaesarShift(parentElement);
                    else if (xmlAttribute.Value == "ReplacementMap")
                        return GenerateReplacementMap(parentElement);
                }
            }

            throw new KeyNotFoundException();
        }

        protected IDistributedCipher GenerateDistributedCipher(XmlElement parentElement, IFerricNode child)
        {
            IList<ICipher> ciphers = null;
            IHeader header = null;
            Guid id = Guid.Empty;

            foreach (XmlAttribute xmlAttribute in parentElement.Attributes)
                if (xmlAttribute.Name == "ID")
                    id = new Guid(xmlAttribute.Value);

            foreach (XmlElement childElement in parentElement)
            {
                if (childElement.Name == "ICiphers")
                    ciphers = GenerateCipherList(childElement);
                else if (childElement.Name == "IHeader")
                    header = GenerateHeader(childElement);
            }

            return new DistributedCipher(id, this.byteSetRepository, header, ciphers, child, FerricHelper.Random);
        }

        protected IList<int> GenerateIndexList(XmlElement parentElement)
        {
            IList<int> indexes = new List<int>();

            foreach (string index in parentElement.InnerText.Split(new char[] { ',' }))
                indexes.Add(Convert.ToInt32(index));

            return indexes;
        }

        protected IFerricNode GenerateFerricNode(XmlElement parentElement, Dictionary<Guid, IFerricNode> ferricNodes)
        {
            IFerricNode child = null;
            IFerricNode ferricNode = null;

            foreach (XmlElement childElement in parentElement)
            {
                if (childElement.Name == "Child")
                {
                    Guid childID = Guid.Empty;

                    foreach (XmlAttribute attribute in childElement.Attributes)
                        if (attribute.Name == "ID")
                            childID = new Guid(attribute.Value);

                    if (childID == Guid.Empty)
                        throw new XmlException("Child was identified but no ID was found for the child.");

                    if(!ferricNodes.ContainsKey(childID))
                        return null;

                    child = ferricNodes[childID];

                    break;
                }
            }

            foreach (XmlAttribute xmlAttribute in parentElement.Attributes)
            {
                if (xmlAttribute.Name == "Type")
                {
                    if (xmlAttribute.Value == "IDistributedCipher")
                        ferricNode = GenerateDistributedCipher(parentElement, child);
                    else if (xmlAttribute.Value == "IScrambler")
                        ferricNode = GenerateScrambler(parentElement, child);
                }
            }

            return ferricNode;
        }

        protected Dictionary<Guid, IFerricNode> GenerateFerricNodes(XmlElement parentElement)
        {
            Dictionary<Guid, IFerricNode> ferricNodes = new Dictionary<Guid, IFerricNode>();

            IList<XmlElement> ferricNodeElements = new List<XmlElement>();
            foreach (XmlElement childElement in parentElement.ChildNodes)
                ferricNodeElements.Add(childElement);

            while (ferricNodeElements.Count > 0)
            {
                IList<XmlElement> processedElements = new List<XmlElement>();

                foreach (XmlElement childElement in ferricNodeElements)
                {
                    IFerricNode ferricNode = GenerateFerricNode(childElement, ferricNodes);

                    if (ferricNode != null)
                    {
                        processedElements.Add(childElement);

                        ferricNodes.Add(ferricNode.ID, ferricNode);
                    }
                }

                foreach (XmlElement processedElement in processedElements)
                    ferricNodeElements.Remove(processedElement);
            }
            
            return ferricNodes;
        }

        protected IHeader GenerateHeader(XmlElement parentElement)
        {
            IByteIndex byteIndex = null;
            string key = null;

            foreach (XmlElement childElement in parentElement)
            {
                if (childElement.Name == "IByteIndex")
                {
                    Guid id = Guid.Empty;

                    foreach (XmlAttribute attribute in childElement.Attributes)
                    {
                        if (attribute.Name == "ID")
                        {
                            id = new Guid(attribute.InnerText);

                            break;
                        }
                    }

                    byteIndex = (IByteIndex)byteSetRepository.Find(id);
                }
                else if (childElement.Name == "Key")
                {
                    foreach (XmlAttribute xmlAttribute in childElement.Attributes)
                    {
                        if (xmlAttribute.Name == "Value")
                        {
                            key = xmlAttribute.Value;

                            break;
                        }
                    }
                }
            }

            return new Header(byteIndex, key);
        }

        protected RandomDataInserts GenerateRandomDataInserts(XmlElement parentElement)
        {
            Guid id = Guid.Empty;

            foreach (XmlElement childElement in parentElement.ChildNodes)
            {
                if (childElement.Name == "IByteIndex")
                {
                    foreach (XmlElement grandchildElement in childElement.ChildNodes)
                    {
                        if (grandchildElement.Name == "ID")
                        {
                            id = new Guid(grandchildElement.InnerText);

                            break;
                        }
                    }

                    break;
                }
            }

            IByteIndex byteIndex = (IByteIndex)this.byteSetRepository.Find(id);

            return new RandomDataInserts(byteIndex);
        }

        protected ReplacementMap GenerateReplacementMap(XmlElement parentElement)
        {
            Guid id = Guid.Empty;

            foreach (XmlElement childElement in parentElement.ChildNodes)
            {
                if (childElement.Name == "IByteMap")
                {
                    foreach (XmlAttribute attribute in childElement.Attributes)
                    {
                        if (attribute.Name == "ID")
                        {
                            id = new Guid(attribute.InnerText);

                            break;
                        }
                    }

                    break;
                }
            }

            IByteMap byteMap = (IByteMap)this.byteSetRepository.Find(id);

            return new ReplacementMap(byteMap);
        }

        protected IScrambler GenerateScrambler(XmlElement parentElement, IFerricNode child)
        {
            Guid id = Guid.Empty;
            IList<int> scrambledIndexes = null;

            foreach (XmlAttribute xmlAttribute in parentElement.Attributes)
                if (xmlAttribute.Name == "ID")
                    id = new Guid(xmlAttribute.Value);

            foreach (XmlElement childElement in parentElement.ChildNodes)
            {
                if (childElement.Name == "ID")
                    id = new Guid(childElement.InnerText);
                else if (childElement.Name == "ScrambledIndexes")
                    scrambledIndexes = GenerateIndexList(childElement);
            }

            return new Scrambler(id, scrambledIndexes, child);
        }

        #endregion XML Element Reading

        protected string GetAssemblyName(object objectInQuestion)
        {
            string fullyQualifiedName = objectInQuestion.GetType().ToString();

            return fullyQualifiedName.Substring(0, fullyQualifiedName.LastIndexOf("."));
        }

        protected string GetTypeName(object objectInQuestion)
        {
            string fullyQualifiedName = objectInQuestion.GetType().ToString();

            int index = fullyQualifiedName.LastIndexOf(".");

            return fullyQualifiedName.Substring(index + 1, fullyQualifiedName.Length - index - 1);
        }
    }
}
