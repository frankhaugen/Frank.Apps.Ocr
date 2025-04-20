using System.Xml.Serialization;

namespace Frank.Apps.Ocr.WebApp.Models.AltoXml;

[XmlRoot(ElementName="TextLine")]
public class TextLine {
    [XmlElement(ElementName="String")]
    public List<String> String { get; set; }
    [XmlElement(ElementName="SP")]
    public List<SP> SP { get; set; }
    [XmlAttribute(AttributeName="ID")]
    public string ID { get; set; }
    [XmlAttribute(AttributeName="HPOS")]
    public string HPOS { get; set; }
    [XmlAttribute(AttributeName="VPOS")]
    public string VPOS { get; set; }
    [XmlAttribute(AttributeName="WIDTH")]
    public string WIDTH { get; set; }
    [XmlAttribute(AttributeName="HEIGHT")]
    public string HEIGHT { get; set; }
}