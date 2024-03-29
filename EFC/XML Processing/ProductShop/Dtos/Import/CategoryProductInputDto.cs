﻿using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Import
{
    [XmlType("CategoryProduct")]
    public class CategoryProductInputDto
    {
        public int CategoryId { get; set; }
        [XmlIgnore]
        public Category[] Category { get; set; }
        public int ProductId { get; set; }
        [XmlIgnore]
        public Product[] Product { get; set; }
    }
}
