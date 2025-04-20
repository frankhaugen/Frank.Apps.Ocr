using System.Xml.Serialization;

namespace Frank.Apps.Ocr.WebApp.Models.AltoXml;

[XmlRoot(ElementName="Styles")]
public class Styles {
    [XmlElement(ElementName="TextStyle")]
    public string TextStyle { get; set; }
    [XmlElement(ElementName="ParagraphStyle")]
    public string ParagraphStyle { get; set; }
}