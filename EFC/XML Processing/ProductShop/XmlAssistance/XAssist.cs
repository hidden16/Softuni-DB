using ProductShop.Dtos.Export;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.XmlAssistance
{
    public class XAssist
    {
        public static string Serialize<T>(T serializerType, string attribute)
        {
            //XmlRootAttribute attribute = new XmlRootAttribute("Products");
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
    }
}
