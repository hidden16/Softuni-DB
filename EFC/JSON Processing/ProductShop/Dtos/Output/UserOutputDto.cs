using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop.Dtos.Output
{
    public class UserOutputDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<SoldProductsDto> SoldProducts { get; set; }
    }
}
