using System.Xml.Serialization;

namespace Frank.Apps.Ocr.WebApp.Models.AltoXml;

[XmlRoot(ElementName="PrintSpace")]
public class PrintSpace {
    [XmlElement(ElementName="ComposedBlock")]
    public List<ComposedBlock> ComposedBlock { get; set; }
    [XmlElement(ElementName="Illustration")]
    public List<Illustration> Illustration { get; set; }
    [XmlElement(ElementName="GraphicalElement")]
    public GraphicalElement GraphicalElement { get; set; }
    [XmlAttribute(AttributeName="HPOS")]
    public string HPOS { get; set; }
    [XmlAttribute(AttributeName="VPOS")]
    public string VPOS { get; set; }
    [XmlAttribute(AttributeName="WIDTH")]
    public string WIDTH { get; set; }
    [XmlAttribute(AttributeName="HEIGHT")]
    public string HEIGHT { get; set; }
}