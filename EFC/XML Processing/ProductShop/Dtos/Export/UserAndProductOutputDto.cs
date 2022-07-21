using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlType("Users")]
    public class UserAndProductOutputDto
    {
        [XmlElement("count")]
        public int Count { get; set; }
        [XmlArray("users")]
        public UserOutputDto[] Users { get; set; }

    }
    [XmlType("User")]
    public class UserOutputDto
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }
        [XmlElement("lastName")]
        public string LastName { get; set; }
        [XmlElement("age")]
        public int? Age { get; set; }
        [XmlElement("SoldProducts")]
        public SoldProductOutputDto SoldProducts { get; set; }

    }
    [XmlType("SoldProducts")]
    public class SoldProductOutputDto
    {
        [XmlElement("count")]
        public int Count { get; set; }
        [XmlArray("products")]
        public SoldProductDto[] Products { get; set; }

    }
}
