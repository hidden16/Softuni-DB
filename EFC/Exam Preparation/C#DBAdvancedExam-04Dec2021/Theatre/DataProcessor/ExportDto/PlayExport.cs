using System.Xml.Serialization;
using Theatre.Data.Models.Enums;

namespace Theatre.DataProcessor.ExportDto
{
    [XmlType("Play")]
    public class PlayExport
    {
        [XmlAttribute]
        public string Title { get; set; }
        [XmlAttribute]
        public string Duration { get; set; }
        [XmlAttribute]
        public string Rating { get; set; }
        [XmlAttribute]
        public Genre Genre { get; set; }
        [XmlArray]
        public PlayCastExport[] Actors { get; set; }
    }
    [XmlType("Actor")]
    public class PlayCastExport
    {
        [XmlAttribute]
        public string FullName { get; set; }
        [XmlAttribute]
        public string MainCharacter { get; set; }
    }
}
