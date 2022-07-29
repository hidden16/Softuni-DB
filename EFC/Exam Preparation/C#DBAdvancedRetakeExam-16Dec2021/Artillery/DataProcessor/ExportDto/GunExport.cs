using System.Xml.Serialization;
namespace Artillery.DataProcessor.ExportDto
{
    [XmlType("Gun")]
    public class GunExport
    {
        [XmlAttribute]
        public string Manufacturer { get; set; }
        [XmlAttribute]
        public string GunType { get; set; }
        [XmlAttribute]
        public int GunWeight { get; set; }
        [XmlAttribute]
        public double BarrelLength { get; set; }
        [XmlAttribute]
        public int Range { get; set; }
        [XmlArray]
        public GunCountryExport[] Countries { get; set; }

    }
    [XmlType("Country")]
    public class GunCountryExport
    {
        [XmlAttribute]
        public string Country { get; set; }
        [XmlAttribute]
        public int ArmySize { get; set; }
    }
}
