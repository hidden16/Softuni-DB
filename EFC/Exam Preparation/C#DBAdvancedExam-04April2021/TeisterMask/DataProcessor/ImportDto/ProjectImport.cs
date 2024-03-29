﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using TeisterMask.Data.Models.Enums;

namespace TeisterMask.DataProcessor.ImportDto
{
    [XmlType("Project")]
    public class ProjectImport
    {
        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Name { get; set; }
        [Required]
        public string OpenDate { get; set; }
        public string DueDate { get; set; }
        [XmlArray]
        public ProjectTaskImport[] Tasks { get; set; }
    }
    [XmlType("Task")]
    public class ProjectTaskImport
    {
        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Name { get; set; }
        [Required]
        public string OpenDate { get; set; }
        [Required]
        public string DueDate { get; set; }
        [Required]
        [Range(0,3)]
        public int ExecutionType { get; set; }
        [Required]
        [Range(0,4)]
        public int LabelType { get; set; }
    }
}
