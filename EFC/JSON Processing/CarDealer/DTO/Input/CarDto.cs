using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.DTO.Input
{
    public class CarDto
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public long TravelledDistance { get; set; }
        public List<int> PartsId { get; set; }
    }
}
