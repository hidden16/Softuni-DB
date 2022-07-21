using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlType("Product")]
    public class ProductsOutputDto
    {
        [XmlElement("name")]
        public string name { get; set; }
        [XmlElement("price")]
        public decimal price { get; set; }
        [XmlElement("buyer")]
        public string buyer { get; set; }
    }
}
