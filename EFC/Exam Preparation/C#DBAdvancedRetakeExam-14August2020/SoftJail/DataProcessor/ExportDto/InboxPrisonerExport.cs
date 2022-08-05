using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ExportDto
{
    [XmlType("Prisoner")]
    public class InboxPrisonerExport
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IncarcerationDate { get; set; }
        [XmlArray]
        public EncryptedMailExport[] EncryptedMessages { get; set; }
    }
    [XmlType("Message")]
    public class EncryptedMailExport
    {
        public string Description { get; set; }
    }
}
