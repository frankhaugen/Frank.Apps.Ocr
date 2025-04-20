using System.Xml.Serialization;

namespace Frank.Apps.Ocr.WebApp.Models.AltoXml;

[XmlRoot(ElementName="alto")]
public class Alto {
    [XmlElement(ElementName="Description")]
    public Description Description { get; set; }
    [XmlElement(ElementName="Styles")]
    public Styles Styles { get; set; }
    [XmlElement(ElementName="Layout")]
    public Layout Layout { get; set; }
}