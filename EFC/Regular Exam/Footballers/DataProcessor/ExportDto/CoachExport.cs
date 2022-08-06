using System.Xml.Serialization;

namespace Footballers.DataProcessor.ExportDto
{
    [XmlType("Coach")]
    public class CoachExport
    {
        [XmlAttribute]
        public int FootballersCount { get; set; }
        public string CoachName { get; set; }
        [XmlArray]
        public CoachFootballerExport[] Footballers { get; set; }
    }
    [XmlType("Footballer")]
    public class CoachFootballerExport
    {
        public string Name { get; set; }
        public string Position { get; set; }
    }
}
