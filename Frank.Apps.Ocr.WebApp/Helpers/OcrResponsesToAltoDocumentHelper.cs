using System.Xml.Serialization;
using Frank.Apps.Ocr.WebApp.Models;
using Frank.Apps.Ocr.WebApp.Models.AltoXml;

namespace Frank.Apps.Ocr.WebApp.Helpers;

public static class OcrResponsesToAltoDocumentHelper
{
    public static string GetRawAltoDocument(OcrResponses ocrResponses)
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
        
        using var stringWriter = new StringWriter();
        var serializer = new XmlSerializer(typeof(Alto));
        serializer.Serialize(stringWriter, alto);
        
        return stringWriter.ToString();
    }
    
    public static Alto GetAltoDocument(OcrResponses ocrResponses)
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