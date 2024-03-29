﻿using SoftJail.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.Data.Models
{
    public class Officer
    {
        public Officer()
        {
            OfficerPrisoners = new List<OfficerPrisoner>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string FullName { get; set; }
        [Required]
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Salary { get; set; }
        [Required]
        public Position Position { get; set; }
        [Required]
        public Weapon Weapon { get; set; }
        [Required]
        public int DepartmentId { get; set; }
        [Required]
        public Department Department { get; set; }
        public ICollection<OfficerPrisoner> OfficerPrisoners { get; set; }
    }
}
