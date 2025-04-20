using System.Xml.Serialization;

namespace Frank.Apps.Ocr.WebApp.Models.AltoXml;

[XmlRoot(ElementName="Layout")]
public class Layout {
    [XmlElement(ElementName="Page")]
    public List<Page> Page { get; set; }
}