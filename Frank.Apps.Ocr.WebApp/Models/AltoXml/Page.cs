using System.Xml.Serialization;

namespace Frank.Apps.Ocr.WebApp.Models.AltoXml;

[XmlRoot(ElementName="Page")]
public class Page {
    [XmlElement(ElementName="TopMargin")]
    public string TopMargin { get; set; }
    [XmlElement(ElementName="LeftMargin")]
    public string LeftMargin { get; set; }
    [XmlElement(ElementName="RightMargin")]
    public string RightMargin { get; set; }
    [XmlElement(ElementName="BottomMargin")]
    public string BottomMargin { get; set; }
    [XmlElement(ElementName="PrintSpace")]
    public PrintSpace PrintSpace { get; set; }
    [XmlAttribute(AttributeName="WIDTH")]
    public string WIDTH { get; set; }
    [XmlAttribute(AttributeName="HEIGHT")]
    public string HEIGHT { get; set; }
    [XmlAttribute(AttributeName="PHYSICAL_IMG_NR")]
    public string PHYSICAL_IMG_NR { get; set; }
    [XmlAttribute(AttributeName="ID")]
    public string ID { get; set; }
}