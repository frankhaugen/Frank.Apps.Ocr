using System.Xml.Serialization;
using Frank.Apps.Ocr.WebApp.Components.Pages;
using Frank.Apps.Ocr.WebApp.Models.AltoXml;

namespace Frank.Apps.Ocr.WebApp.Helpers;

public static class OcrResponsesToAltoDocumentHelper
{
    public static Alto GetAltoDocument(Home.OcrResponses ocrResponses)
    {
        var pageSerializer = new XmlSerializer(typeof(Page));
        
        var alto = new Alto
        {
            Description = new Description
            {
                MeasurementUnit = "pixel"
            },
            Layout = new Layout
            {
                Page = new List<Page>()
            }
        };
        
        foreach (var ocrResponsesPage in ocrResponses.Pages)
        {
            if (pageSerializer.Deserialize(new StringReader(ocrResponsesPage.AltoText ?? string.Empty)) is Page deserializedPage)
            {
                alto.Layout.Page.Add(deserializedPage);
            }
        }
        
        return alto;
    }
}