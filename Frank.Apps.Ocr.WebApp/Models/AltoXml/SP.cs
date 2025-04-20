using System.Xml.Serialization;

namespace Frank.Apps.Ocr.WebApp.Models.AltoXml;

[XmlRoot(ElementName="SP")]
public class SP {
    [XmlAttribute(AttributeName="WIDTH")]
    public string WIDTH { get; set; }
    [XmlAttribute(AttributeName="VPOS")]
    public string VPOS { get; set; }
    [XmlAttribute(AttributeName="HPOS")]
    public string HPOS { get; set; }
}