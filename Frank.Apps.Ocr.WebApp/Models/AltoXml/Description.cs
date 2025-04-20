using System.Xml.Serialization;

namespace Frank.Apps.Ocr.WebApp.Models.AltoXml;

[XmlRoot(ElementName="Description")]
	public class Description {
		[XmlElement(ElementName="MeasurementUnit")]
		public string MeasurementUnit { get; set; }
		[XmlElement(ElementName="sourceImageInformation")]
		public string SourceImageInformation { get; set; }
		[XmlElement(ElementName="Processing")]
		public string Processing { get; set; }
	}