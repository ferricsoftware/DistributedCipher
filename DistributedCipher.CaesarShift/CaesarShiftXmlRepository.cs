using System;
using System.Xml;

using DistributedCipher.Common;

namespace DistributedCipher.CaesarShift
{
    public class CaesarShiftXmlRepository
    {
        //protected XmlNode CreateCaesarShiftElement(CaesarShift caesarShift, XmlDocument xmlDocument)
        //{
        //    XmlElement caesarShiftElement = CreateCipherTypeElement(caesarShift, xmlDocument);

        //    if (caesarShift.ShiftDirection != ShiftDirection.Right)
        //    {
        //        XmlElement shiftDirectionElement = xmlDocument.CreateElement("ShiftDirection");

        //        shiftDirectionElement.AppendChild(xmlDocument.CreateTextNode(caesarShift.ShiftDirection.ToString()));

        //        caesarShiftElement.AppendChild(shiftDirectionElement);
        //    }

        //    if (caesarShift.Amount != 1)
        //    {
        //        XmlElement amountElement = xmlDocument.CreateElement("Amount");

        //        amountElement.AppendChild(xmlDocument.CreateTextNode(caesarShift.Amount.ToString()));

        //        caesarShiftElement.AppendChild(amountElement);
        //    }

        //    return caesarShiftElement;
        //}
    }
}
