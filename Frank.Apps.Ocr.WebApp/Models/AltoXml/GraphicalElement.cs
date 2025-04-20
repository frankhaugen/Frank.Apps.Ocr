using System.Xml.Serialization;

namespace Frank.Apps.Ocr.WebApp.Models.AltoXml;

[XmlRoot(ElementName="GraphicalElement")]
public class GraphicalElement {
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