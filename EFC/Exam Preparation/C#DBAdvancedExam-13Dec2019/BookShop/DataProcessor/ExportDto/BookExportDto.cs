using System;
using System.Xml.Serialization;

namespace BookShop.DataProcessor.ExportDto
{
    [XmlType("Book")]
    public class BookExportDto
    {
        [XmlElement]
        public string Name { get; set; }
        [XmlElement]
        public string Date { get; set; }
        [XmlAttribute]
        public int Pages { get; set; }
    }
}
