using Microsoft.Xrm.Sdk;
using System;
using System.Net;
using System.Web;
using System.Xml;

namespace Ariv.Dynamics.BingGeocoding.Plugin
{
    public class BingWebClient : WebClient
    {
        private const string BASE_URL = "http://dev.virtualearth.net/REST/v1/Locations";
        private readonly string key;

        public BingWebClient(string key)
        {
            this.key = key;
        }

        protected override WebRequest GetWebRequest(Uri url)
        {
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(url);
            if (request != null)
            {
                request.Timeout = 15000;
                request.KeepAlive = false;
            }
            return request;
        }

        public Coordinates GetCoordinates(string address)
        {
            var coordinates = default(Coordinates);

            var uriBuilder = new UriBuilder(BASE_URL);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            
            query["query"] = address;
            query["key"] = this.key;
            query["maxResults"] = "1";
            query["o"] = "xml";
            uriBuilder.Query = Uri.EscapeUriString(HttpUtility.UrlDecode(query.ToString()));

            var request = this.GetWebRequest(uriBuilder.Uri);
            var xmlDocument = this.GetXMLResponse(request);

            if(xmlDocument != null)
            {
                coordinates = this.ExtractCoordinatesFromXml(xmlDocument);
            }

            return coordinates;
        }

        private XmlDocument GetXMLResponse(WebRequest request)
        {
            var xmlDocument = default(XmlDocument);
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    xmlDocument = new XmlDocument();
                    xmlDocument.Load(response.GetResponseStream());
                }
            }
            return xmlDocument;
        }

        private Coordinates ExtractCoordinatesFromXml(XmlDocument xmlDocument)
        {
            var coordinates = default(Coordinates);
            XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            xmlNamespaceManager.AddNamespace("rest", "http://schemas.microsoft.com/search/local/ws/rest/v1");

            XmlNodeList pointElements = xmlDocument.SelectNodes("//rest:Point", xmlNamespaceManager);

            if(pointElements.Count == 1)
            {
                coordinates = new Coordinates()
                {
                    Latitude = Convert.ToDouble(pointElements[0].SelectSingleNode(".//rest:Latitude", xmlNamespaceManager).InnerText),
                    Longitude = Convert.ToDouble(pointElements[0].SelectSingleNode(".//rest:Longitude", xmlNamespaceManager).InnerText)
                };
            }

            return coordinates;
        }
    }
}
