using Newtonsoft.Json;
using System.Xml;

namespace IB_Domain
{
    public class IBDomain
    {
        //Convert Xml To Json
        public string ConvertXmlToJson()
        {
            //Initialize XmlDocument to read Xml file
            XmlDocument xmlFile = new();
            xmlFile.Load(@"C:\Users\WIMTRUCK 2\Desktop\IEI\DataDemo\XmlFile.xml");

            //Convert Xml to Json with library Newtonsoft.Json
            string json = JsonConvert.SerializeXmlNode(xmlFile);

            return json;
        }
    }
}