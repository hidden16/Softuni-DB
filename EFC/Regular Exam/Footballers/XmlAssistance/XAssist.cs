using System.IO;
using System.Text;
using System.Xml.Serialization;
namespace Footballers.XmlAssistance
{
    public class XAssist
    {
        public static string Serialize<T>(T serializerType, string attribute)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(attribute));

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            var builder = new StringBuilder();
            using (var write = new StringWriter(builder))
            {
                serializer.Serialize(write, serializerType, namespaces);
            }
            return builder.ToString().TrimEnd();
        }
        public static T[] Deserialize<T>(string rootAttribute, string inputXml)
        {
            XmlRootAttribute attribute = new XmlRootAttribute(rootAttribute);
            XmlSerializer serializer = new XmlSerializer(typeof(T[]), attribute);
            StringReader reader = new StringReader(inputXml);
            var deserializedReturn = serializer.Deserialize(reader) as T[];
            return deserializedReturn;
        }
    }
}
