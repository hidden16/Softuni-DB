using System.Xml.Serialization;

namespace VaporStore.DataProcessor.Dto.Export
{
    [XmlType("User")]
    public class UserExport
    {
        [XmlAttribute("username")]
        public string Username { get; set; }
        [XmlArray]
        public UserPurchaseExport[] Purchases { get; set; }
        public decimal TotalSpent { get; set; }

    }
    [XmlType("Purchase")]
    public class UserPurchaseExport
    {
        public string Card { get; set; }
        public string Cvc { get; set; }
        public string Date { get; set; }
        public UserPurchaseGameExport Game { get; set; }
    }
    [XmlType("Game")]
    public class UserPurchaseGameExport
    {
        [XmlAttribute("title")]
        public string Title { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
    }
}
